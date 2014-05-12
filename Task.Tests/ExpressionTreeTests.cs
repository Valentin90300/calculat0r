using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Task;

namespace Task.Tests.ExpressionTreeTestsNamespace
{
    #region Test Stuff
    public class SomePriority : IPriorityCategory
    {
        public PriorityCategoryRegisterBase PriorityCategoryRegister
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public int GetPriorityNumber()
        {
            throw new NotImplementedException();
        }
    }

    public class IntOperand : NumberLexemeBase
    {
        public Int32 Value { get; set; }

        public IntOperand(Int32 value) : base(value)
        {
            this.Value = value;
        }
    }

    public class SumOperator : BinaryOperatorLexemeBase
    {
        public SumOperator() : base(new SomePriority())
        {
        }

        public override NumberLexemeBase Perform(NumberLexemeBase operandA, NumberLexemeBase operandB)
        {
            Assert.IsNotNull(operandA);
            Assert.IsInstanceOfType(operandA, typeof(IntOperand));
            Assert.IsNotNull(operandB);
            Assert.IsInstanceOfType(operandB, typeof(IntOperand));

            var intOpA = operandA as IntOperand;
            var intOpB = operandB as IntOperand;

            var intOpRes = new IntOperand(intOpA.Value + intOpB.Value);
            return intOpRes;
        }
    }

    public class InverseOperator : UnaryOperatorLexemeBase
    {
        public InverseOperator() : base(new SomePriority())
        {
        }

        public override NumberLexemeBase Perform(NumberLexemeBase operand)
        {
            Assert.IsNotNull(operand);
            Assert.IsInstanceOfType(operand, typeof(IntOperand));

            var intOp = operand as IntOperand;

            var intOpRes = new IntOperand(-intOp.Value);
            return intOpRes;
        }
    }
    #endregion

    [TestClass]
    public class ExpressionTreeTests
    {
        [TestMethod]
        public void CreateExpressionTreeAndCalculateResultTest()
        {
            // Arrange
            var intOperandA = new IntOperand(1);
            var intOperandB = new IntOperand(2);
            var intOperandC = new IntOperand(3);

            var rootSumOperator = new SumOperator();
            var leftChildSumOperator = new SumOperator();
            var rightChildInverseOperator = new InverseOperator();

            IExpressionTree expressionTree = new ExpressionTree();

            // Act
            var rootSumOperatorNode = expressionTree.CreateRootNode(rootSumOperator);
            var leftChildSumOperatorNode = expressionTree.CreateChildNode
            (
                rootSumOperatorNode, ParentBranchKind.Left, leftChildSumOperator
            );
            var rightChildInverseOperatorNode = expressionTree.CreateChildNode
            (
                rootSumOperatorNode, ParentBranchKind.Right, rightChildInverseOperator
            );

            expressionTree.CreateChildNode
            (
                leftChildSumOperatorNode, ParentBranchKind.Left, intOperandA
            );
            expressionTree.CreateChildNode
            (
                leftChildSumOperatorNode, ParentBranchKind.Right, intOperandB
            );
            expressionTree.CreateChildNode
            (
                rightChildInverseOperatorNode, ParentBranchKind.Right, intOperandC
            );

            var calculatedValue = expressionTree.Calculate();

            // Assert
            Assert.IsNotNull(calculatedValue);
            Assert.IsInstanceOfType(calculatedValue, typeof(IntOperand));
            Assert.AreEqual(0, (calculatedValue as IntOperand).Value);
        }
    }
}
