using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;

namespace Task
{
    internal class ExpressionTree : IExpressionTree
    {
        private ExpressionTreeNode rootNode;

        internal ExpressionTree()
        {
            this.rootNode = null;
        }

        ExpressionTreeNode IExpressionTree.CreateRootNode(ILexeme value)
        {
            this.rootNode = new ExpressionTreeNode(null, value);
            return this.rootNode;
        }

        ExpressionTreeNode IExpressionTree.CreateChildNode
        (
            ExpressionTreeNode parentNode,
            ParentBranchKind parentBranchKind,
            ILexeme value
        )
        {
            var node = new ExpressionTreeNode(parentNode, value);
            if (parentBranchKind == ParentBranchKind.Left)
            {
                parentNode.LeftChild = node;
            }
            else
            {
                parentNode.RightChild = node;
            }

            return node;
        }

        NumberLexemeBase IExpressionTree.Calculate()
        {
            this.VisitNode(this.rootNode);
            return this.rootNode.Value as NumberLexemeBase;
        }

        private void VisitNode(ExpressionTreeNode currentNode)
        {
            if (currentNode.LeftChild != null)
            {
                this.VisitNode(currentNode.LeftChild);
            }

            if (currentNode.RightChild != null)
            {
                this.VisitNode(currentNode.RightChild);
            }

            if (currentNode.Value is OperatorLexemeBase)
            {
                NumberLexemeBase result;

                if (currentNode.Value is UnaryOperatorLexemeBase)
                {
                    var unaryOperator = currentNode.Value as UnaryOperatorLexemeBase;
                    var operand = currentNode.RightChild.Value as NumberLexemeBase;

                    result = unaryOperator.Perform(operand);
                }
                else
                {
                    var binaryOperator = currentNode.Value as BinaryOperatorLexemeBase;
                    var leftOperand = currentNode.LeftChild.Value as NumberLexemeBase;
                    var rightOperand = currentNode.RightChild.Value as NumberLexemeBase;

                    result = binaryOperator.Perform(leftOperand, rightOperand);
                }

                currentNode.Update(result);
            }
        }
    }
}
