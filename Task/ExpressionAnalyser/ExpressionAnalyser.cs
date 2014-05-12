using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;

namespace Task
{
    internal class ExpressionAnalyser : IExpressionAnalyser
    {
        private readonly IExpressionLexemeFactory expressionLexemefactory;

        internal ExpressionAnalyser()
            :
            this(new ExpressionLexemeFactory())
        {
        }

        internal ExpressionAnalyser(IExpressionLexemeFactory expressionLexemeFactory)
        {
            this.expressionLexemefactory = expressionLexemeFactory;
        }

        IExpressionLexeme IExpressionAnalyser.Parse(LinkedList<ILexeme> lexemeList)
        {
            return this.Parse(lexemeList, true);
        }

        private IExpressionLexeme Parse(LinkedList<ILexeme> lexemeList, Boolean isTopLevel)
        {
            var content = new LinkedList<ILexeme>();
            while (true)
            {
                var currentNode = lexemeList.First;
                if (currentNode == null)
                {
                    if (isTopLevel)
                    {
                        if (content.Count == 0)
                        {
                            throw new EmptyExpressionException();
                        }

                        return this.expressionLexemefactory.CreateExpressionLexeme(content);
                    }
                    else
                    {
                        throw new OneOrMoreClosingBracketMissingException();
                    }
                }

                lexemeList.Remove(currentNode);

                if (currentNode.Value is OpeningBracketLexeme)
                {
                    var expressionLexeme = this.Parse(lexemeList, false);
                    content.AddLast(expressionLexeme);
                }
                else if (currentNode.Value is ClosingBracketLexeme)
                {
                    if (isTopLevel == true)
                    {
                        throw new OneOrMoreOpeningBracketMissingException();
                    }

                    if (content.Count == 0)
                    {
                        throw new EmptyExpressionException();
                    }

                    var expressionLexeme = this.expressionLexemefactory.CreateExpressionLexeme(content);
                    return expressionLexeme;
                }
                else
                {
                    content.AddLast(currentNode);
                }
            }
        }
    }
}
