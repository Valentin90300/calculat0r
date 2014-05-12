using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task
{
    internal interface IExpressionLexemeFactory
    {
        IExpressionLexeme CreateExpressionLexeme(LinkedList<ILexeme> content);
    }
}
