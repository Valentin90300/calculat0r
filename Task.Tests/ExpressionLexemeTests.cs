using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Task;

namespace Task.Tests.ExpressionLexemeTestsNamespace
{
    #region Test Stuff
    public class LowOperatorPriority : IPriorityCategory
    {
        public PriorityCategoryRegisterBase PriorityCategoryRegister
        {
	        get { throw new NotImplementedException(); }
	        set { throw new NotImplementedException(); }
        }

        public int GetPriorityNumber()
        {
 	        return 1;
        }
    }

    public class MiddleOperatorPriority : IPriorityCategory
    {
        public PriorityCategoryRegisterBase PriorityCategoryRegister
        {
	        get { throw new NotImplementedException(); }
	        set { throw new NotImplementedException(); }
        }

        public int GetPriorityNumber()
        {
 	        return 2;
        }
    }

    public class HighOperatorPriority : IPriorityCategory
    {
        public PriorityCategoryRegisterBase PriorityCategoryRegister
        {
	        get { throw new NotImplementedException(); }
	        set { throw new NotImplementedException(); }
        }

        public int GetPriorityNumber()
        {
 	        return 3;
        }
    }

    public class xBinaryOperatorLexeme : BinaryOperatorLexemeBase
    {
        public xBinaryOperatorLexeme() : base(new LowOperatorPriority())
        {
        }

        public override NumberLexemeBase Perform(NumberLexemeBase operandA, NumberLexemeBase operandB)
        {
            throw new NotImplementedException();
        }
    }

    public class yBinaryOperatorLexeme : BinaryOperatorLexemeBase
    {
        public yBinaryOperatorLexeme() : base(new MiddleOperatorPriority())
        {
        }

        public override NumberLexemeBase Perform(NumberLexemeBase operandA, NumberLexemeBase operandB)
        {
            throw new NotImplementedException();
        }
    }

    public class zBinaryOperatorLexeme : BinaryOperatorLexemeBase
    {
        public zBinaryOperatorLexeme() : base(new HighOperatorPriority())
        {
        }

        public override NumberLexemeBase Perform(NumberLexemeBase operandA, NumberLexemeBase operandB)
        {
            throw new NotImplementedException();
        }
    }

    public class iUnaryOperatorLexeme : UnaryOperatorLexemeBase
    {
        public iUnaryOperatorLexeme() : base(new HighOperatorPriority()) 
        { 
        }
    
        public override NumberLexemeBase Perform(NumberLexemeBase operand)
        {
 	        throw new NotImplementedException();
        }
    }

    public class jUnaryOperatorLexeme : UnaryOperatorLexemeBase
    {
        public jUnaryOperatorLexeme() : base(new LowOperatorPriority()) 
        { 
        }

        public override NumberLexemeBase Perform(NumberLexemeBase operand)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
    
    [TestClass]
    public class ExpressionLexemeTests
    {
        [TestMethod]
        public void SuccessfullySplitExpressionTest()
        {
            // Arrange
            var aLexeme = Mock.Of<ILexeme>();
            var bLexeme = Mock.Of<ILexeme>();
            var cLexeme = Mock.Of<ILexeme>();
            var dLexeme = Mock.Of<ILexeme>();

            var xOpLexeme = new xBinaryOperatorLexeme();
            var yOpLexeme = new yBinaryOperatorLexeme();
            var zOpLexeme = new zBinaryOperatorLexeme();
            var iOpLexeme = new iUnaryOperatorLexeme();

            var content = new LinkedList<ILexeme>
            (
                new ILexeme[]
                {
                    aLexeme,
                    yOpLexeme,
                    bLexeme,
                    xOpLexeme,
                    cLexeme,
                    zOpLexeme,
                    iOpLexeme,
                    dLexeme
                }
            );

            IExpressionLexeme expressionLexeme = new ExpressionLexeme(content);

            // Act
            BinaryOperatorLexemeBase binaryOperatorLexeme;
            IExpressionLexeme leftExpressionLexeme;
            IExpressionLexeme rightExpressionLexeme;

            expressionLexeme.SplitExpressionByBinaryOperatorWithLowestPriority
            (
                out binaryOperatorLexeme,
                out leftExpressionLexeme,
                out rightExpressionLexeme
            );

            // Assert
            Assert.IsNotNull(binaryOperatorLexeme);
            Assert.AreEqual(xOpLexeme, binaryOperatorLexeme);

            Assert.IsNotNull(leftExpressionLexeme);
            Assert.IsNotNull(rightExpressionLexeme);

            var leftExpressionExpectedContent = new ILexeme[]
            {
                aLexeme,
                yOpLexeme,
                bLexeme
            };

            var rightExpressionExpectedContent = new ILexeme[]
            {
                cLexeme,
                zOpLexeme,
                iOpLexeme,
                dLexeme
            };

            CollectionAssert.AreEqual(leftExpressionExpectedContent, leftExpressionLexeme.ToArray());
            CollectionAssert.AreEqual(rightExpressionExpectedContent, rightExpressionLexeme.ToArray());
        }

        [TestMethod]
        public void UnsuccessfullySplitExpressionTest()
        {
            // Arrange
            var iOpLexeme = new iUnaryOperatorLexeme();
            var aLexeme = Mock.Of<ILexeme>();

            var content = new LinkedList<ILexeme>
            (
                new ILexeme[]
                {
                    iOpLexeme,
                    aLexeme
                }
            );

            IExpressionLexeme expressionLexeme = new ExpressionLexeme(content);

            // Act
            BinaryOperatorLexemeBase binaryOperatorLexeme;
            IExpressionLexeme leftExpressionLexeme;
            IExpressionLexeme rightExpressionLexeme;

            expressionLexeme.SplitExpressionByBinaryOperatorWithLowestPriority
            (
                out binaryOperatorLexeme,
                out leftExpressionLexeme,
                out rightExpressionLexeme
            );

            // Assert
            Assert.IsNull(binaryOperatorLexeme);
            Assert.IsNull(leftExpressionLexeme);
            Assert.IsNull(rightExpressionLexeme);
        }

        [TestMethod]
        public void SuccessfullyTakeUnaryOperatorTest()
        {
            // Arrange
            var iOpLexeme = new iUnaryOperatorLexeme();
            var jOpLexeme = new jUnaryOperatorLexeme();
            var aLexeme = Mock.Of<ILexeme>();

            var content = new LinkedList<ILexeme>
            (
                new ILexeme[]
                {
                    iOpLexeme,
                    jOpLexeme,
                    aLexeme
                }
            );

            IExpressionLexeme expressionLexeme = new ExpressionLexeme(content);

            // Act
            var unaryOperatorLexeme = expressionLexeme.TakeUnaryOperatorWithLowestPriority();

            // Assert
            Assert.IsNotNull(unaryOperatorLexeme);
            Assert.AreEqual(jOpLexeme, unaryOperatorLexeme);

            var sourceModifiedExpressionExpectedContent = new ILexeme[]
            {
                    iOpLexeme,
                    aLexeme
            };

            CollectionAssert.AreEqual(sourceModifiedExpressionExpectedContent, expressionLexeme.ToArray());
        }

        [TestMethod]
        public void UnsuccessfullyTakeUnaryOperatorTest()
        {
            // Arrange
            var aLexeme = Mock.Of<ILexeme>();

            var content = new LinkedList<ILexeme>
            (
                new ILexeme[]
                {
                    aLexeme
                }
            );

            IExpressionLexeme expressionLexeme = new ExpressionLexeme(content);

            // Act
            var unaryOperatorLexeme = expressionLexeme.TakeUnaryOperatorWithLowestPriority();

            // Assert
            Assert.IsNull(unaryOperatorLexeme);
        }
    }
}
