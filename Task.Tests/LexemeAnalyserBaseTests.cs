using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Task;

namespace Task.Tests.LexemeAnalyserTestsNamespace
{
    #region Test Stuff
    public class TestLexeme : ILexeme
    {
        public LexemeKind LexemeKindSource
        {
            get; set;
        }

        public LexemeKind LexemeKind 
        { 
            get { return this.LexemeKindSource; } 
        }

        public String LexemeString { get; set; }
    }

    public class TestLexemeAnalyser : LexemeAnalyserBase
    {
        private readonly TransitionTable transitionTable;

        public static LexemeKind LexemeKindStatic
        {
            get; set;
        }

        public TestLexemeAnalyser(TransitionTable transitionTable)
        {
            this.transitionTable = transitionTable;
        }

        protected override TransitionTable TransitionTable
        {
            get { return this.transitionTable; }
        }

        protected override ILexeme CreateLexeme(string lexemeString)
        {
            return new TestLexeme() { LexemeKindSource = LexemeKindStatic, LexemeString = lexemeString };
        }
    }
    #endregion

    [TestClass]
    public class LexemeAnalyserBaseTests
    {
        [TestMethod]
        public void FixedLengthValidLexemeParseTest()
        {
            // Arrange
            TestLexemeAnalyser.LexemeKindStatic = LexemeKind.WithFixedLength;

            var transitions = new Transition[]
            {
                new Transition(0, 1, DecimalDigitCharCategory.Get(), TransitionKind.Straight),
                new Transition(1, 2, DecimalDigitCharCategory.Get(), TransitionKind.Straight),
            };
            var transitionTable = new TransitionTable(3, transitions);
            var testLexemeAnalyser = new TestLexemeAnalyser(transitionTable);

            // Act / Assert
            Assert.AreEqual(LexemeAnalyserStatus.InProgress, testLexemeAnalyser.Status);
            TestHelper.ExceptionThrowingCheck<NoAvailableParsedLexemeException>
            (
                () => { var r = testLexemeAnalyser.ParsedLexeme; }
            );

            testLexemeAnalyser.Step('1');

            Assert.AreEqual(LexemeAnalyserStatus.InProgress, testLexemeAnalyser.Status);
            TestHelper.ExceptionThrowingCheck<NoAvailableParsedLexemeException>
            (
                () => { var r = testLexemeAnalyser.ParsedLexeme; }
            );

            testLexemeAnalyser.Step('4');

            Assert.AreEqual(LexemeAnalyserStatus.Completed, testLexemeAnalyser.Status);

            var parsedLexeme = testLexemeAnalyser.ParsedLexeme;

            Assert.IsNotNull(parsedLexeme);
            Assert.IsInstanceOfType(parsedLexeme, typeof(TestLexeme));
            Assert.AreEqual("14", (parsedLexeme as TestLexeme).LexemeString); 
        }

        [TestMethod]
        public void VariableLengthValidLexemeParseTest()
        {
            // Arrange
            TestLexemeAnalyser.LexemeKindStatic = LexemeKind.WithVariableLength;

            var transitions = new Transition[]
            {
                new Transition(0, 1, AsteriskCharCategory.Get(), TransitionKind.Straight),
                new Transition(1, 1, AsteriskCharCategory.Get(), TransitionKind.Straight),
                new Transition(1, 2, AsteriskCharCategory.Get(), TransitionKind.Inverse)
            };
            var transitionTable = new TransitionTable(3, transitions);
            var testLexemeAnalyser = new TestLexemeAnalyser(transitionTable);

            // Act / Assert
            Assert.AreEqual(LexemeAnalyserStatus.InProgress, testLexemeAnalyser.Status);
            TestHelper.ExceptionThrowingCheck<NoAvailableParsedLexemeException>
            (
                () => { var r = testLexemeAnalyser.ParsedLexeme; }
            );

            testLexemeAnalyser.Step('*');

            Assert.AreEqual(LexemeAnalyserStatus.InProgress, testLexemeAnalyser.Status);
            TestHelper.ExceptionThrowingCheck<NoAvailableParsedLexemeException>
            (
                () => { var r = testLexemeAnalyser.ParsedLexeme; }
            );

            testLexemeAnalyser.Step('*');

            Assert.AreEqual(LexemeAnalyserStatus.InProgress, testLexemeAnalyser.Status);
            TestHelper.ExceptionThrowingCheck<NoAvailableParsedLexemeException>
            (
                () => { var r = testLexemeAnalyser.ParsedLexeme; }
            );

            testLexemeAnalyser.Step('.');

            Assert.AreEqual(LexemeAnalyserStatus.Completed, testLexemeAnalyser.Status);

            var parsedLexeme = testLexemeAnalyser.ParsedLexeme;

            Assert.IsNotNull(parsedLexeme);
            Assert.IsInstanceOfType(parsedLexeme, typeof(TestLexeme));
            Assert.AreEqual("**", (parsedLexeme as TestLexeme).LexemeString); 
        }

        [TestMethod]
        public void InvalidLexemeParseTest()
        {
            // Arrange
            TestLexemeAnalyser.LexemeKindStatic = LexemeKind.WithFixedLength;

            var transitions = new Transition[]
            {
                new Transition(0, 1, SignCharCategory.Get(), TransitionKind.Straight),
                new Transition(1, 2, DecimalDigitCharCategory.Get(), TransitionKind.Straight),
            };
            var transitionTable = new TransitionTable(3, transitions);
            var testLexemeAnalyser = new TestLexemeAnalyser(transitionTable);

            // Act / Assert
            Assert.AreEqual(LexemeAnalyserStatus.InProgress, testLexemeAnalyser.Status);
            TestHelper.ExceptionThrowingCheck<NoAvailableParsedLexemeException>
            (
                () => { var r = testLexemeAnalyser.ParsedLexeme; }
            );

            testLexemeAnalyser.Step('+');

            Assert.AreEqual(LexemeAnalyserStatus.InProgress, testLexemeAnalyser.Status);
            TestHelper.ExceptionThrowingCheck<NoAvailableParsedLexemeException>
            (
                () => { var r = testLexemeAnalyser.ParsedLexeme; }
            );

            testLexemeAnalyser.Step('.');

            Assert.AreEqual(LexemeAnalyserStatus.Failed, testLexemeAnalyser.Status);
            TestHelper.ExceptionThrowingCheck<NoAvailableParsedLexemeException>
            (
                () => { var r = testLexemeAnalyser.ParsedLexeme; }
            );
        }

        [TestMethod]
        public void ResetTest()
        {
            // Arrange
            TestLexemeAnalyser.LexemeKindStatic = LexemeKind.WithFixedLength;

            var transitions = new Transition[]
            {
                new Transition(0, 1, DecimalDigitCharCategory.Get(), TransitionKind.Straight),
                new Transition(1, 2, PlusCharCategory.Get(), TransitionKind.Straight),
            };
            var transitionTable = new TransitionTable(3, transitions);
            var testLexemeAnalyser = new TestLexemeAnalyser(transitionTable);

            // Act
            testLexemeAnalyser.Step('9');
            testLexemeAnalyser.Step('+');
            testLexemeAnalyser.Reset();

            // Assert
            Assert.AreEqual(LexemeAnalyserStatus.InProgress, testLexemeAnalyser.Status);
            TestHelper.ExceptionThrowingCheck<NoAvailableParsedLexemeException>
            (
                () => { var r = testLexemeAnalyser.ParsedLexeme; }
            );
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidAccessToStepMethodException))]
        public void InvalidAccessToStepMethodTest()
        {
            // Arrange
            TestLexemeAnalyser.LexemeKindStatic = LexemeKind.WithFixedLength;
            
            var transitions = new Transition[]
            {
                new Transition(0, 1, DotCharCategory.Get(), TransitionKind.Straight)
            };
            var transitionTable = new TransitionTable(2, transitions);
            var testLexemeAnalyser = new TestLexemeAnalyser(transitionTable);

            // Act
            testLexemeAnalyser.Step('.');
            testLexemeAnalyser.Step('@');
        }

        [TestMethod]
        [ExpectedException(typeof(MoreThanOneTransitionAvailableException))]
        public void MoreThanOneTransitionAvailableTest()
        {
            // Arrange
            TestLexemeAnalyser.LexemeKindStatic = LexemeKind.WithFixedLength;

            var transitions = new Transition[]
            {
                new Transition(0, 1, SignCharCategory.Get(), TransitionKind.Straight),
                new Transition(0, 1, PlusCharCategory.Get(), TransitionKind.Straight)
            };
            var transitionTable = new TransitionTable(2, transitions);
            var testLexemeAnalyser = new TestLexemeAnalyser(transitionTable);

            // Act
            testLexemeAnalyser.Step('+');
        }
    }
}
