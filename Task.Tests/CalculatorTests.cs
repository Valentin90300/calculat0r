using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Task;

namespace Task.Tests.CalculatorTestsNamespace
{
    #region Test Stuff
    public class TestNumberLexeme : NumberLexemeBase
    {
        public TestNumberLexeme()
            :
            base(100)
        {
        }
    }

    public class TestPriorityCategoryRegister : PriorityCategoryRegisterBase
    {
        protected override void SetupPriorityCategoryList()
        {
            var priorityCategoryMock = new Mock<IPriorityCategory>();
            this.Add(priorityCategoryMock.Object);
        }
    }
    #endregion

    [TestClass]
    public class CalculatorTests
    {
        [TestMethod]
        public void CalculateTest()
        {
            // Arrange
            var lexemeLinkedList = new LinkedList<ILexeme>();

            var testLexicalAnalyserMock = new Mock<ILexicalAnalyser>();
            testLexicalAnalyserMock.Setup(m => m.Parse(It.IsAny<BinaryReader>())).Returns<BinaryReader>
            (
                rdr =>
                {
                    Assert.IsNotNull(rdr);
                    Assert.IsTrue(rdr.BaseStream.CanSeek);

                    var streamContent = rdr.ReadChars(3);

                    Assert.AreEqual(3, streamContent.Length);
                    Assert.AreEqual('a', streamContent[0]);
                    Assert.AreEqual('b', streamContent[1]);
                    Assert.AreEqual('c', streamContent[2]);

                    return lexemeLinkedList;
                }
            );

            var testNumberLexeme = new TestNumberLexeme();

            var testExpressionTreeMock = new Mock<IExpressionTree>(MockBehavior.Strict);
            testExpressionTreeMock.Setup(m => m.Calculate()).Returns(testNumberLexeme);

            var testSyntaxAnalyserMock = new Mock<ISyntaxAnalyser>();
            testSyntaxAnalyserMock.Setup(m => m.Parse(It.IsAny<LinkedList<ILexeme>>())).Returns<LinkedList<ILexeme>>
            (
                lst =>
                {
                    Assert.IsNotNull(lst);
                    Assert.AreEqual(lexemeLinkedList, lst);

                    return testExpressionTreeMock.Object;
                }
            );

            var testPriorityCategoryRegister = new TestPriorityCategoryRegister();

            var calculator = new Calculator
            (
                testLexicalAnalyserMock.Object, 
                testSyntaxAnalyserMock.Object,
                testPriorityCategoryRegister
            );

            var expressionString = "abc";

            // Act
            var resultedObject = calculator.Calculate(expressionString);

            // Assert
            Assert.IsNotNull(resultedObject);
            Assert.AreEqual(testNumberLexeme.PackedNumber, resultedObject);
        }

        [TestMethod]
        [ExpectedException(typeof(ExpressionStringNullException))]
        public void ExpressionNullTest()
        {
            // Arrange
            var testLexicalAnalyser = Mock.Of<ILexicalAnalyser>();
            var testSyntaxAnalyser = Mock.Of<ISyntaxAnalyser>();
            var testPriorityCategoryRegister = new TestPriorityCategoryRegister();

            var calculator = new Calculator(testLexicalAnalyser, testSyntaxAnalyser, testPriorityCategoryRegister);

            // Act
            calculator.Calculate(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ExpressionStringDoesNotContainSignificantCharsException))]
        public void ExpressionContainsOnlyWhitespaceCharsTest()
        {
            // Arrange
            var testLexicalAnalyser = Mock.Of<ILexicalAnalyser>();
            var testSyntaxAnalyser = Mock.Of<ISyntaxAnalyser>();
            var testPriorityCategoryRegister = new TestPriorityCategoryRegister();

            var calculator = new Calculator(testLexicalAnalyser, testSyntaxAnalyser, testPriorityCategoryRegister);

            // Act
            calculator.Calculate("   \0");
        }
    }
}
