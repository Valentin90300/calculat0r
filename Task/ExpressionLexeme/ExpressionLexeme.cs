using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task
{
    internal class ExpressionLexeme : IExpressionLexeme
    {
        private readonly LinkedList<ILexeme> content;

        LexemeKind ILexeme.LexemeKind
        {
            get { return LexemeKind.WithVariableLength; }
        }

        internal ExpressionLexeme(LinkedList<ILexeme> content)
        {
            this.content = content;
        }

        IEnumerator<ILexeme> IEnumerable<ILexeme>.GetEnumerator()
        {
            return this.content.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (this.content as IEnumerable).GetEnumerator();
        }

        void IExpressionLexeme.SplitExpressionByBinaryOperatorWithLowestPriority
        (
            out BinaryOperatorLexemeBase binaryOperatorLexeme,
            out IExpressionLexeme leftExpressionLexeme,
            out IExpressionLexeme rightExpressionLexeme
        )
        {
            LinkedListNode<ILexeme> splitPointNode = null;
            LinkedListNode<ILexeme> currentNode = this.content.First;

            while (currentNode != null)
            {
                var operatorLexeme = currentNode.Value as BinaryOperatorLexemeBase;
                if (operatorLexeme != null)
                {
                    if (splitPointNode == null)
                    {
                        splitPointNode = currentNode;
                    }
                    else
                    {
                        var splitPointLexeme = splitPointNode.Value as BinaryOperatorLexemeBase;
                        if 
                        (
                            operatorLexeme.PriorityCategory.GetPriorityNumber() 
                            <= 
                            splitPointLexeme.PriorityCategory.GetPriorityNumber()
                        )
                        {
                            splitPointNode = currentNode;
                        }
                    }
                }

                currentNode = currentNode.Next;
            }

            if (splitPointNode == null)
            {
                binaryOperatorLexeme = null;
                leftExpressionLexeme = null;
                rightExpressionLexeme = null;
            }
            else
            {
                binaryOperatorLexeme = splitPointNode.Value as BinaryOperatorLexemeBase;
                leftExpressionLexeme = new ExpressionLexeme
                (
                    this.CreateSubLinkedList(this.content.First, splitPointNode.Previous)
                );
                rightExpressionLexeme = new ExpressionLexeme
                (
                    this.CreateSubLinkedList(splitPointNode.Next, this.content.Last)
                );
            }
        }

        UnaryOperatorLexemeBase IExpressionLexeme.TakeUnaryOperatorWithLowestPriority()
        {
            LinkedListNode<ILexeme> targetOpNode = null;
            LinkedListNode<ILexeme> node = this.content.First;

            do
            {
                if (node.Value is UnaryOperatorLexemeBase)
                {
                    if (targetOpNode == null)
                    {
                        targetOpNode = node;
                    }
                    else
                    {
                        if
                        (
                            (node.Value as UnaryOperatorLexemeBase).PriorityCategory.GetPriorityNumber()
                            <
                            (targetOpNode.Value as UnaryOperatorLexemeBase).PriorityCategory.GetPriorityNumber()
                        )
                        {
                            targetOpNode = node;
                        }
                    }
                }

                node = node.Next;
            }
            while (node != null);

            if (targetOpNode == null)
            {
                return null;
            }
            else
            {
                this.content.Remove(targetOpNode);
                return targetOpNode.Value as UnaryOperatorLexemeBase;
            }
        }

        private LinkedList<ILexeme> CreateSubLinkedList(LinkedListNode<ILexeme> nodeFrom, LinkedListNode<ILexeme> nodeTo)
        {
            var newLinkedList = new LinkedList<ILexeme>();
            
            var currentNode = nodeFrom;
            while (currentNode != nodeTo.Next)
            {
                newLinkedList.AddLast(currentNode.Value);
                currentNode = currentNode.Next;
            }

            return newLinkedList;
        }
    }
}
