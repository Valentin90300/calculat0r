using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task
{
    internal interface IExpressionAnalyser
    {
        IExpressionLexeme Parse(LinkedList<ILexeme> lexemeList);
    }
}
