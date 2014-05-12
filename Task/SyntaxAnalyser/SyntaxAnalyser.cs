using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;

namespace Task
{
    internal class SyntaxAnalyser : ISyntaxAnalyser
    {
        private readonly IExpressionAnalyser expressionAnalyser;
        private readonly IExpressionTree expressionTree;

        internal SyntaxAnalyser()
            :
            this(new ExpressionAnalyser(), new ExpressionTree())
        {
        }

        internal SyntaxAnalyser(IExpressionAnalyser expressionAnalyser, IExpressionTree expressionTree)
        {
            this.expressionAnalyser = expressionAnalyser;
            this.expressionTree = expressionTree;
        }

        IExpressionTree ISyntaxAnalyser.Parse(LinkedList<ILexeme> lexemeList)
        {
            var rootExpressionLexeme = this.expressionAnalyser.Parse(lexemeList);
            this.Parse(rootExpressionLexeme, null, ParentBranchKind.None);
            return this.expressionTree;
        }

        private void Parse
        (
            IExpressionLexeme expressionLexeme, ExpressionTreeNode parentNode, ParentBranchKind parentBranchKind
        )
        {
            ExpressionTreeNode newNode;

            BinaryOperatorLexemeBase binaryOperatorLexeme;
            IExpressionLexeme leftExpressionLexeme;
            IExpressionLexeme rightExpressionLexeme;

            expressionLexeme.SplitExpressionByBinaryOperatorWithLowestPriority
            (
                out binaryOperatorLexeme, out leftExpressionLexeme, out rightExpressionLexeme
            );

            if (binaryOperatorLexeme != null)
            {
                newNode = this.CreateNode(parentNode, parentBranchKind, binaryOperatorLexeme);

                this.Parse(leftExpressionLexeme, newNode, ParentBranchKind.Left);
                this.Parse(rightExpressionLexeme, newNode, ParentBranchKind.Right);

                return;
            }

            ILexeme lastInnerLexeme = null;
            ILexeme lastInnerOperandLexeme = null;

            foreach (var l in expressionLexeme)
            {
                lastInnerLexeme = l;
                if (l is NumberLexemeBase || l is IExpressionLexeme)
                {
                    lastInnerOperandLexeme = l;
                }
            }

            if (lastInnerOperandLexeme == null || lastInnerOperandLexeme != lastInnerLexeme)
            {
                throw new ExpressionDoesNotContainOperandLexemeException();
            }

            var unaryOperatorLexeme = expressionLexeme.TakeUnaryOperatorWithLowestPriority();
            if (unaryOperatorLexeme == null)
            {
                if (lastInnerLexeme is IExpressionLexeme)
                {
                    this.Parse(lastInnerLexeme as IExpressionLexeme, parentNode, parentBranchKind);
                }
                else
                {
                    newNode = this.CreateNode(parentNode, parentBranchKind, lastInnerLexeme);
                }
            }
            else
            {
                newNode = this.CreateNode(parentNode, parentBranchKind, unaryOperatorLexeme);
                this.Parse(expressionLexeme, newNode, ParentBranchKind.Right);
            }
        }

        private ExpressionTreeNode CreateNode
        (
            ExpressionTreeNode parentNode, ParentBranchKind parentBranchKind, ILexeme lexeme
        )
        {
            if (parentNode == null)
            {
                return this.expressionTree.CreateRootNode(lexeme);
            }
            else
            {
                return this.expressionTree.CreateChildNode(parentNode, parentBranchKind, lexeme);
            }
        }
    }
}
