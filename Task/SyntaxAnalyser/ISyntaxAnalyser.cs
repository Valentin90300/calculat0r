using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task
{
    internal interface ISyntaxAnalyser
    {
        IExpressionTree Parse(LinkedList<ILexeme> lexemeList);
    }
}
