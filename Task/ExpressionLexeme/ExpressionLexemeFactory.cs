using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task
{
    internal class ExpressionLexemeFactory : IExpressionLexemeFactory
    {
        IExpressionLexeme IExpressionLexemeFactory.CreateExpressionLexeme(LinkedList<ILexeme> content)
        {
            return new ExpressionLexeme(content);
        }
    }
}
