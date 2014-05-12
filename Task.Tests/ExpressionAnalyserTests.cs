using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Task;

namespace Task.Tests.ExpressionAnalyserTestsNamespace
{
    #region Test Stuff
    public class ALexeme : ILexeme 
    { 
        public LexemeKind LexemeKind 
        { 
            get { return LexemeKind.WithFixedLength; } 
        } 
    }

    public class BLexeme : ILexeme
    {
        public LexemeKind LexemeKind 
        { 
            get { return LexemeKind.WithFixedLength; } 
        }
    }

    public class CLexeme : ILexeme 
    {
        public LexemeKind LexemeKind 
        { 
            get { return LexemeKind.WithFixedLength; } 
        }
    }

    public class DLexeme : ILexeme
    {
        public LexemeKind LexemeKind 
        { 
            get { return LexemeKind.WithFixedLength; } 
        }
    }

    public class TestExpressionLexeme : IExpressionLexeme
    {
        public LexemeKind LexemeKind 
        { 
            get { return LexemeKind.WithVariableLength; } 
        }

        public LinkedList<ILexeme> Content { get; set; }

        public TestExpressionLexeme(LinkedList<ILexeme> content)
        {
            this.Content = content;
        }

        public IEnumerator<ILexeme> GetEnumerator()
        {
            return this.Content.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (this.Content as IEnumerable).GetEnumerator();
        }

        void IExpressionLexeme.SplitExpressionByBinaryOperatorWithLowestPriority
        (
            out BinaryOperatorLexemeBase binaryOperatorLexeme, 
            out IExpressionLexeme leftExpressionLexeme, 
            out IExpressionLexeme rightExpressionLexeme
        )
        {
            throw new AssertFailedException("Непредусмотренный вызов метода объекта зависимости IExpressionLexeme.");
        }

        UnaryOperatorLexemeBase IExpressionLexeme.TakeUnaryOperatorWithLowestPriority()
        {
            throw new AssertFailedException("Непредусмотренный вызов метода объекта зависимости IExpressionLexeme.");
        }
    }
    #endregion

    [TestClass]
    public class ExpressionAnalyserTests
    {
        [TestMethod]
        public void SimpleExpressionParseTest()
        {
            // Arrange
            IExpressionAnalyser expressionAnalyser = new ExpressionAnalyser(this.CreateExpressionLexemeFactory());
            var lexemeLinkedList = new LinkedList<ILexeme>
            (
                new ILexeme[]
                {
                    new ALexeme(),
                    new BLexeme()
                }
            );

            // Act
            var expressionLexeme = expressionAnalyser.Parse(lexemeLinkedList);

            // Assert
            Assert.IsNotNull(expressionLexeme);
            Assert.IsInstanceOfType(expressionLexeme, typeof(TestExpressionLexeme));

            // Act
            var content = (expressionLexeme as TestExpressionLexeme).Content;

            // Assert
            Assert.IsNotNull(content);
            Assert.AreEqual(2, content.Count);
            Assert.IsNotNull(content.First.Value);
            Assert.IsInstanceOfType(content.First.Value, typeof(ALexeme));
            Assert.IsNotNull(content.Last.Value);
            Assert.IsInstanceOfType(content.Last.Value, typeof(BLexeme));
        }

        [TestMethod]
        public void ComplexExpressionParseTest()
        {
            // Arrange
            IExpressionAnalyser expressionAnalyser = new ExpressionAnalyser(this.CreateExpressionLexemeFactory());
            var lexemeLinkedList = new LinkedList<ILexeme>
            (
                new ILexeme[]
                {
                    new ALexeme(),
                    new OpeningBracketLexeme(),
                    new BLexeme(),
                    new CLexeme(),
                    new ClosingBracketLexeme(),
                    new DLexeme()
                }
            );

            // Act
            var outerExpressionLexeme = expressionAnalyser.Parse(lexemeLinkedList);

            // Assert
            Assert.IsNotNull(outerExpressionLexeme);
            Assert.IsInstanceOfType(outerExpressionLexeme, typeof(TestExpressionLexeme));

            // Act
            var outerContent = (outerExpressionLexeme as TestExpressionLexeme).Content;

            // Assert
            Assert.IsNotNull(outerContent);
            Assert.AreEqual(3, outerContent.Count);
            Assert.IsNotNull(outerContent.First.Value);
            Assert.IsInstanceOfType(outerContent.First.Value, typeof(ALexeme));
            Assert.IsNotNull(outerContent.Last.Value);
            Assert.IsInstanceOfType(outerContent.Last.Value, typeof(DLexeme));

            // Act
            var innerExpressionLexeme = outerContent.ElementAt(1);
            
            // Assert
            Assert.IsNotNull(innerExpressionLexeme);
            Assert.IsInstanceOfType(innerExpressionLexeme, typeof(TestExpressionLexeme));

            // Act
            var innerContent = (innerExpressionLexeme as TestExpressionLexeme).Content;

            // Assert
            Assert.IsNotNull(innerContent);
            Assert.AreEqual(2, innerContent.Count);
            Assert.IsNotNull(innerContent.First.Value);
            Assert.IsInstanceOfType(innerContent.First.Value, typeof(BLexeme));
            Assert.IsNotNull(innerContent.Last.Value);
            Assert.IsInstanceOfType(innerContent.Last.Value, typeof(CLexeme));
        }

        [TestMethod]
        [ExpectedException(typeof(OneOrMoreOpeningBracketMissingException))]
        public void MissingOpeningBracketTest()
        {
            // Arrange
            IExpressionAnalyser expressionAnalyser = new ExpressionAnalyser(this.CreateExpressionLexemeFactory());
            var lexemeLinkedList = new LinkedList<ILexeme>
            (
                new ILexeme[]
                {
                    new ALexeme(),
                    new BLexeme(),
                    new CLexeme(),
                    new ClosingBracketLexeme(),
                    new DLexeme()
                }
            );

            // Act
            var outerExpressionLexeme = expressionAnalyser.Parse(lexemeLinkedList);
        }

        [TestMethod]
        [ExpectedException(typeof(OneOrMoreClosingBracketMissingException))]
        public void MissingClosingBracketTest()
        {
            // Arrange
            IExpressionAnalyser expressionAnalyser = new ExpressionAnalyser(this.CreateExpressionLexemeFactory());
            var lexemeLinkedList = new LinkedList<ILexeme>
            (
                new ILexeme[]
                {
                    new ALexeme(),
                    new OpeningBracketLexeme(),
                    new BLexeme(),
                    new CLexeme(),
                    new DLexeme()
                }
            );

            // Act
            var outerExpressionLexeme = expressionAnalyser.Parse(lexemeLinkedList);
        }

        [TestMethod]
        [ExpectedException(typeof(EmptyExpressionException))]
        public void EmptyExpressionTest()
        {
            // Arrange
            IExpressionAnalyser expressionAnalyser = new ExpressionAnalyser(this.CreateExpressionLexemeFactory());
            var lexemeLinkedList = new LinkedList<ILexeme>
            (
                new ILexeme[]
                {
                    new ALexeme(),
                    new OpeningBracketLexeme(),
                    new ClosingBracketLexeme(),
                    new DLexeme()
                }
            );

            // Act
            var outerExpressionLexeme = expressionAnalyser.Parse(lexemeLinkedList);
        }

        private IExpressionLexemeFactory CreateExpressionLexemeFactory()
        {
            var expressionLexemeFactoryMock = new Mock<IExpressionLexemeFactory>();
            expressionLexemeFactoryMock.Setup
            (
                m => m.CreateExpressionLexeme(It.IsAny<LinkedList<ILexeme>>())
            )
            .Returns<LinkedList<ILexeme>>
            (
                content =>
                {
                    Assert.IsNotNull(content);
                    Assert.AreNotEqual(0, content.Count);
                    Assert.IsFalse(content.Any(l => l == null));

                    return new TestExpressionLexeme(content);
                }
            );

            return expressionLexemeFactoryMock.Object;
        }
    }
}
