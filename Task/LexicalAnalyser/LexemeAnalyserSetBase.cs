using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task
{
    public abstract class LexemeAnalyserSetBase : IEnumerable<ILexemeAnalyser>
    {
        private readonly List<ILexemeAnalyser> lexemeAnalyserSet;

        public LexemeAnalyserSetBase()
        {
            this.lexemeAnalyserSet = new List<ILexemeAnalyser>();
            
            this.SetupLexemeAnalyserSet();
            if (this.lexemeAnalyserSet.Count == 0)
            {
                throw new EmptyLexemeAnalyserListException();
            }
        }

        public IEnumerator<ILexemeAnalyser> GetEnumerator()
        {
            return this.lexemeAnalyserSet.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (this.lexemeAnalyserSet as IEnumerable).GetEnumerator();
        }

        protected void Add(ILexemeAnalyser lexemeAnalyser)
        {
            if (lexemeAnalyser == null)
            {
                throw new LexemeAnalyserParameterNullException();
            }

            if (this.lexemeAnalyserSet.Contains(lexemeAnalyser))
            {
                throw new LexemeAnalyserObjectAlreadyAddedException();
            }
            
            #if !DEBUG
            if (this.lexemeAnalyserSet.Select(la => la.GetType()).Contains(lexemeAnalyser.GetType()))
            {
                throw new LexemeAnalyserTypeAlreadyAddedException();
            }
            #endif

            this.lexemeAnalyserSet.Add(lexemeAnalyser);
        }

        protected abstract void SetupLexemeAnalyserSet();
    }
}