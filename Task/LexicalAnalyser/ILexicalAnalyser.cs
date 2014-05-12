using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Task
{
    internal interface ILexicalAnalyser
    {
        LinkedList<ILexeme> Parse(BinaryReader expressionReader);
    }
}
