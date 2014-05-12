using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task
{
    public class DefaultLexemeAnalyserSet : LexemeAnalyserSetBase
    {
        protected override void SetupLexemeAnalyserSet()
        {
            this.Add(new Int32LexemeAnalyser());
            this.Add(new DoubleLexemeAnalyser());
            this.Add(new UnaryMinusLexemeAnalyser());
            this.Add(new UnaryPlusLexemeAnalyser());
            this.Add(new AdditionLexemeAnalyser());
            this.Add(new SubtractionLexemeAnalyser());
            this.Add(new MultiplicationLexemeAnalyser());
            this.Add(new DivisionLexemeAnalyser());
            this.Add(new OpeningBracketLexemeAnalyser());
            this.Add(new ClosingBracketLexemeAnalyser());
        }
    }
}
