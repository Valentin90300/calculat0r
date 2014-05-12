using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task
{
    public interface ILexemeAnalyser
    {
        LexemeAnalyserStatus Status { get; }
        ILexeme ParsedLexeme { get; }

        void Reset();
        void Step(Char @char);
    }
}
