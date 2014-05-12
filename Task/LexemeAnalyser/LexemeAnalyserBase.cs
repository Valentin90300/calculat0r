using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task
{
    public abstract class LexemeAnalyserBase : ILexemeAnalyser
    {
        private Int32 currentStateIndex;
        private LexemeAnalyserStatus status;
        private StringBuilder lexemeStringBuilder;
        private ILexeme parsedLexeme;

        public LexemeAnalyserStatus Status
        {
            get { return this.status; }
        }

        public ILexeme ParsedLexeme
        {
            get
            {
                if (this.status != LexemeAnalyserStatus.Completed)
                {
                    throw new NoAvailableParsedLexemeException();
                }
                return this.parsedLexeme;
            }
        }

        protected abstract TransitionTable TransitionTable { get; }

        public LexemeAnalyserBase()
        {
            this.lexemeStringBuilder = new StringBuilder();
            this.Reset();
        }

        protected abstract ILexeme CreateLexeme(String lexemeString);

        public void Reset()
        {
            this.currentStateIndex = 0;
            this.status = LexemeAnalyserStatus.InProgress;
            this.lexemeStringBuilder.Clear();
            this.parsedLexeme = null;
        }

        public void Step(Char @char)
        {
            if (this.status != LexemeAnalyserStatus.InProgress)
            {
                throw new InvalidAccessToStepMethodException();
            }

            var appropriateTransitions = this.TransitionTable.Transitions.Where
            (
                t => this.IsAppropriateTransition(t, @char)
            )
            .ToArray();

            if (appropriateTransitions.Count() == 1)
            {
                var currentTransition = appropriateTransitions.First();
                this.currentStateIndex = currentTransition.StateToIndex;

                if (this.currentStateIndex == (this.TransitionTable.StateCount - 1))
                {
                    if (currentTransition.TransitionKind == TransitionKind.Straight)
                    {
                        this.lexemeStringBuilder.Append(@char);
                    }

                    try
                    {
                        this.parsedLexeme = this.CreateLexeme(this.lexemeStringBuilder.ToString());
                        this.status = LexemeAnalyserStatus.Completed;
                    }
                    catch (ArgumentException exc)
                    {
                        this.currentStateIndex = -1;
                        this.status = LexemeAnalyserStatus.Failed;

                        throw exc;
                    }
                }
                else
                {
                    this.lexemeStringBuilder.Append(@char);
                }
            }
            else
            {
                this.currentStateIndex = -1;
                this.status = LexemeAnalyserStatus.Failed;

                if (appropriateTransitions.Count() > 1)
                {
                    throw new MoreThanOneTransitionAvailableException();
                }
            }
        }

        private Boolean IsAppropriateTransition(Transition transition, Char @char)
        {
            Boolean isAppropriateState = transition.StateFromIndex == this.currentStateIndex;

            Boolean isAppropriateChar = transition.CharCategory.Contains(@char);
            if (transition.TransitionKind == TransitionKind.Inverse)
            {
                isAppropriateChar = !isAppropriateChar;
            }

            return isAppropriateState && isAppropriateChar;
        }
    }
}
