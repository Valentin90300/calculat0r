using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Task;

namespace Task.Tests.LexicalAnalyserTestsNamespace
{
    #region Test Stuff
    public class AXLexeme : ILexeme 
    { 
        public LexemeKind LexemeKind { get { return LexemeKind.WithFixedLength; } } 
    }

    public class ABLexeme : ILexeme 
    { 
        public LexemeKind LexemeKind { get { return LexemeKind.WithFixedLength; } } 
    }

    public class ALexeme : ILexeme 
    { 
        public LexemeKind LexemeKind { get { return LexemeKind.WithFixedLength; } } 
    }

    public class TestLexemeAnalyserSet : LexemeAnalyserSetBase
    {
        public static ILexemeAnalyser[] LexemeAnalyserArray { get; set; }

        protected override void SetupLexemeAnalyserSet()
        {
            foreach (var la in TestLexemeAnalyserSet.LexemeAnalyserArray)
            {
                this.Add(la);
            }
        }
    }
    #endregion

    [TestClass]
    public class LexicalAnalyserTests
    {
        [TestMethod]
        public void CompetitionParseModelTest()
        {
            // Arrange
            TestLexemeAnalyserSet.LexemeAnalyserArray = new ILexemeAnalyser[]
            {
                this.CreateAXLexemeAnalyser(),
                this.CreateABLexemeAnalyser()
            };

            var lexemeAnalyserSet = new TestLexemeAnalyserSet();

            ILexicalAnalyser lexicalAnalyser = new LexicalAnalyser(lexemeAnalyserSet);

            // Act
            var lexemeLinkedList = lexicalAnalyser.Parse(this.CreateBinaryReaderForString("ab\0"));

            // Assert
            Assert.IsNotNull(lexemeLinkedList);
            Assert.AreEqual(1, lexemeLinkedList.Count);
            Assert.IsNotNull(lexemeLinkedList.First.Value);
            Assert.IsInstanceOfType(lexemeLinkedList.First.Value, typeof(ABLexeme));
        }

        
        [TestMethod]
        public void GreedyParseModelTest()
        {
            // Arrange
            TestLexemeAnalyserSet.LexemeAnalyserArray = new ILexemeAnalyser[]
            {
                this.CreateALexemeAnalyser(),
                this.CreateABLexemeAnalyser()
            };

            var lexemeAnalyserSet = new TestLexemeAnalyserSet();

            ILexicalAnalyser lexicalAnalyser = new LexicalAnalyser(lexemeAnalyserSet);

            // Act
            var lexemeLinkedList = lexicalAnalyser.Parse(this.CreateBinaryReaderForString("ab\0"));

            // Assert
            Assert.IsNotNull(lexemeLinkedList);
            Assert.AreEqual(1, lexemeLinkedList.Count);
            Assert.IsNotNull(lexemeLinkedList.First.Value);
            Assert.IsInstanceOfType(lexemeLinkedList.First.Value, typeof(ABLexeme));
        }

        
        [TestMethod]
        [ExpectedException(typeof(UnknownLexemeException))]
        public void UnknowLexemeTest()
        {
            // Arrange
            TestLexemeAnalyserSet.LexemeAnalyserArray = new ILexemeAnalyser[]
            {
                this.CreateABLexemeAnalyser(),
                this.CreateAXLexemeAnalyser()
            };

            var lexemeAnalyserSet = new TestLexemeAnalyserSet();

            ILexicalAnalyser lexicalAnalyser = new LexicalAnalyser(lexemeAnalyserSet);

            // Act
            var lexemeLinkedList = lexicalAnalyser.Parse(this.CreateBinaryReaderForString("ar\0"));
        }

        [TestMethod]
        [ExpectedException(typeof(SimultaneousSeveralLexemeAnalysersCompletedException))]
        public void SimultaneousSeveralLexemeAnalysersCompletedTest()
        {
            // Arrange
            TestLexemeAnalyserSet.LexemeAnalyserArray = new ILexemeAnalyser[]
            {
                this.CreateALexemeAnalyser(),
                this.CreateALexemeAnalyser()
            };

            var lexemeAnalyserSet = new TestLexemeAnalyserSet();

            ILexicalAnalyser lexicalAnalyser = new LexicalAnalyser(lexemeAnalyserSet);

            // Act
            var lexemeLinkedList = lexicalAnalyser.Parse(this.CreateBinaryReaderForString("a\0"));
        }

        private BinaryReader CreateBinaryReaderForString(String expressionString)
        {
            var memoryStream = new MemoryStream
            (
                Encoding.UTF8.GetBytes(expressionString)
            );

            var binaryReader = new BinaryReader(memoryStream);
            return binaryReader;
        }

        /// <summary>
        /// Создает тестовый парсер для тестовой лексемы "ax"
        /// </summary>
        /// <returns></returns>
        private ILexemeAnalyser CreateAXLexemeAnalyser()
        {
            var axLexemeAnalyserMockStateIndex = 0;
            var axLexemeAnalyserMock = new Mock<ILexemeAnalyser>();

            axLexemeAnalyserMock.Setup(m => m.Step(It.IsAny<Char>())).Callback<Char>
            (
                @char =>
                {
                    if (axLexemeAnalyserMockStateIndex == 0)
                    {
                        if (@char == 'a')
                        {
                            axLexemeAnalyserMockStateIndex = 1;
                        }
                        else
                        {
                            axLexemeAnalyserMockStateIndex = -1;
                        }
                    }
                    else if (axLexemeAnalyserMockStateIndex == 1)
                    {
                        if (@char == 'x')
                        {
                            axLexemeAnalyserMockStateIndex = 2;
                        }
                        else
                        {
                            axLexemeAnalyserMockStateIndex = -1;
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
            );
            axLexemeAnalyserMock.Setup(m => m.Reset()).Callback
            (
                () => axLexemeAnalyserMockStateIndex = 0
            );
            axLexemeAnalyserMock.SetupGet(m => m.Status).Returns
            (
                () =>
                {
                    if (axLexemeAnalyserMockStateIndex == 0 || axLexemeAnalyserMockStateIndex == 1)
                    {
                        return LexemeAnalyserStatus.InProgress;
                    }
                    else if (axLexemeAnalyserMockStateIndex == 2)
                    {
                        return LexemeAnalyserStatus.Completed;
                    }
                    else
                    {
                        return LexemeAnalyserStatus.Failed;
                    }
                }
            );
            axLexemeAnalyserMock.SetupGet(m => m.ParsedLexeme).Returns
            (
                () =>
                {
                    if (axLexemeAnalyserMockStateIndex == 2)
                    {
                        return new AXLexeme();
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
            );

            return axLexemeAnalyserMock.Object;
        }


        /// <summary>
        /// Создает тестовый парсер для тестовой лексемы "ab"
        /// </summary>
        /// <returns></returns>
        private ILexemeAnalyser CreateABLexemeAnalyser()
        {
            var abLexemeAnalyserMockStateIndex = 0;
            var abLexemeAnalyserMock = new Mock<ILexemeAnalyser>();

            abLexemeAnalyserMock.Setup(m => m.Step(It.IsAny<Char>())).Callback<Char>
            (
                @char =>
                {
                    if (abLexemeAnalyserMockStateIndex == 0)
                    {
                        if (@char == 'a')
                        {
                            abLexemeAnalyserMockStateIndex = 1;
                        }
                        else
                        {
                            abLexemeAnalyserMockStateIndex = -1;
                        }
                    }
                    else if (abLexemeAnalyserMockStateIndex == 1)
                    {
                        if (@char == 'b')
                        {
                            abLexemeAnalyserMockStateIndex = 2;
                        }
                        else
                        {
                            abLexemeAnalyserMockStateIndex = -1;
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
            );
            abLexemeAnalyserMock.Setup(m => m.Reset()).Callback
            (
                () => abLexemeAnalyserMockStateIndex = 0
            );
            abLexemeAnalyserMock.SetupGet(m => m.Status).Returns
            (
                () =>
                {
                    if (abLexemeAnalyserMockStateIndex == 0 || abLexemeAnalyserMockStateIndex == 1)
                    {
                        return LexemeAnalyserStatus.InProgress;
                    }
                    else if (abLexemeAnalyserMockStateIndex == 2)
                    {
                        return LexemeAnalyserStatus.Completed;
                    }
                    else
                    {
                        return LexemeAnalyserStatus.Failed;
                    }
                }
            );
            abLexemeAnalyserMock.SetupGet(m => m.ParsedLexeme).Returns
            (
                () =>
                {
                    if (abLexemeAnalyserMockStateIndex == 2)
                    {
                        return new ABLexeme();
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
            );

            return abLexemeAnalyserMock.Object;
        }


        /// <summary>
        /// Создает тестовый парсер для тестовой лексемы "a"
        /// </summary>
        /// <returns></returns>
        private ILexemeAnalyser CreateALexemeAnalyser()
        {
            var aLexemeAnalyserMockStateIndex = 0;
            var aLexemeAnalyserMock = new Mock<ILexemeAnalyser>();

            aLexemeAnalyserMock.Setup(m => m.Step(It.IsAny<Char>())).Callback<Char>
            (
                @char =>
                {
                    if (aLexemeAnalyserMockStateIndex == 0)
                    {
                        if (@char == 'a')
                        {
                            aLexemeAnalyserMockStateIndex = 1;
                        }
                        else
                        {
                            aLexemeAnalyserMockStateIndex = -1;
                        }
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
            );
            aLexemeAnalyserMock.Setup(m => m.Reset()).Callback
            (
                () => aLexemeAnalyserMockStateIndex = 0
            );
            aLexemeAnalyserMock.SetupGet(m => m.Status).Returns
            (
                () =>
                {
                    if (aLexemeAnalyserMockStateIndex == 0)
                    {
                        return LexemeAnalyserStatus.InProgress;
                    }
                    else if (aLexemeAnalyserMockStateIndex == 1)
                    {
                        return LexemeAnalyserStatus.Completed;
                    }
                    else
                    {
                        return LexemeAnalyserStatus.Failed;
                    }
                }
            );
            aLexemeAnalyserMock.SetupGet(m => m.ParsedLexeme).Returns
            (
                () =>
                {
                    if (aLexemeAnalyserMockStateIndex == 1)
                    {
                        return new ALexeme();
                    }
                    else
                    {
                        throw new InvalidOperationException();
                    }
                }
            );

            return aLexemeAnalyserMock.Object;
        }
    }
}
