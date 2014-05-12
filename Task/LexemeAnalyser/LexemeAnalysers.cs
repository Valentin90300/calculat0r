using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Task
{
    public class Int32LexemeAnalyser : LexemeAnalyserBase
    {
        private readonly TransitionTable transitionTable;

        public Int32LexemeAnalyser()
        {
            var transitions = new Transition[]
            {
                new Transition(0, 1, DecimalDigitCharCategory.Get(), TransitionKind.Straight),
                new Transition(1, 1, DecimalDigitCharCategory.Get(), TransitionKind.Straight),
                new Transition(1, 2, DecimalDigitCharCategory.Get(), TransitionKind.Inverse)
            };

            this.transitionTable = new TransitionTable(3, transitions);
        }

        protected override TransitionTable TransitionTable
        {
            get { return this.transitionTable; }
        }

        protected override ILexeme CreateLexeme(String lexemeString)
        {
            Int32 parsedInt32Number;
            if 
            (
                !Int32.TryParse
                (
                    lexemeString, 
                    NumberStyles.Integer, 
                    CultureInfo.InvariantCulture.NumberFormat, 
                    out parsedInt32Number
                )
            )
            {
                throw new Int32StringParseException();
            }

            return new Int32Lexeme(parsedInt32Number);
        }
    }

    public class DoubleLexemeAnalyser : LexemeAnalyserBase
    {
        private readonly TransitionTable transitionTable;

        public DoubleLexemeAnalyser()
        {
            var transitions = new Transition[]
            {
                new Transition(0, 1, DecimalDigitCharCategory.Get(), TransitionKind.Straight),
                new Transition(1, 1, DecimalDigitCharCategory.Get(), TransitionKind.Straight),
                new Transition(1, 2, DotCharCategory.Get(), TransitionKind.Straight),
                new Transition(2, 3, DecimalDigitCharCategory.Get(), TransitionKind.Straight),
                new Transition(3, 3, DecimalDigitCharCategory.Get(), TransitionKind.Straight),
                new Transition(3, 4, DecimalDigitCharCategory.Get(), TransitionKind.Inverse),
                new Transition(2, 4, DecimalDigitCharCategory.Get(), TransitionKind.Inverse)
            };

            this.transitionTable = new TransitionTable(5, transitions);
        }

        protected override TransitionTable TransitionTable
        {
            get { return this.transitionTable; }
        }

        protected override ILexeme CreateLexeme(String lexemeString)
        {
            Double parsedDoubleNumber;
            if 
            (
                !Double.TryParse
                (
                    lexemeString, 
                    NumberStyles.Float | NumberStyles.AllowThousands, 
                    CultureInfo.InvariantCulture.NumberFormat,
                    out parsedDoubleNumber
                )
            )
            {
                throw new DoubleStringParseException();
            }

            return new DoubleLexeme(parsedDoubleNumber);
        }
    }

    public class UnaryPlusLexemeAnalyser : LexemeAnalyserBase
    {
        private readonly TransitionTable transitionTable;

        public UnaryPlusLexemeAnalyser()
        {
            var transitions = new Transition[]
            {
                new Transition(0, 1, PlusCharCategory.Get(), TransitionKind.Straight)
            };

            this.transitionTable = new TransitionTable(2, transitions);
        }

        protected override TransitionTable TransitionTable
        {
            get { return this.transitionTable; }
        }

        protected override ILexeme CreateLexeme(String lexemeString)
        {
            return new UnaryPlusLexeme();
        }
    }

    public class UnaryMinusLexemeAnalyser : LexemeAnalyserBase
    {
        private readonly TransitionTable transitionTable;

        public UnaryMinusLexemeAnalyser()
        {
            var transitions = new Transition[]
            {
                new Transition(0, 1, MinusCharCategory.Get(), TransitionKind.Straight)
            };

            this.transitionTable = new TransitionTable(2, transitions);
        }

        protected override TransitionTable TransitionTable
        {
            get { return this.transitionTable; }
        }

        protected override ILexeme CreateLexeme(String lexemeString)
        {
            return new UnaryMinusLexeme();
        }
    }

    public class AdditionLexemeAnalyser : LexemeAnalyserBase
    {
        private readonly TransitionTable transitionTable;

        public AdditionLexemeAnalyser()
        {
            var transitions = new Transition[]
            {
                new Transition(0, 1, PlusCharCategory.Get(), TransitionKind.Straight)
            };

            this.transitionTable = new TransitionTable(2, transitions);
        }

        protected override TransitionTable TransitionTable
        {
            get { return this.transitionTable; }
        }

        protected override ILexeme CreateLexeme(String lexemeString)
        {
            return new AdditionLexeme();
        }
    }

    public class SubtractionLexemeAnalyser : LexemeAnalyserBase
    {
        private readonly TransitionTable transitionTable;

        public SubtractionLexemeAnalyser()
        {
            var transitions = new Transition[]
            {
                new Transition(0, 1, MinusCharCategory.Get(), TransitionKind.Straight)
            };

            this.transitionTable = new TransitionTable(2, transitions);
        }

        protected override TransitionTable TransitionTable
        {
            get { return this.transitionTable; }
        }

        protected override ILexeme CreateLexeme(String lexemeString)
        {
            return new SubtractionLexeme();
        }
    }

    public class MultiplicationLexemeAnalyser : LexemeAnalyserBase
    {
        private readonly TransitionTable transitionTable;

        public MultiplicationLexemeAnalyser()
        {
            var transitions = new Transition[]
            {
                new Transition(0, 1, AsteriskCharCategory.Get(), TransitionKind.Straight)
            };

            this.transitionTable = new TransitionTable(2, transitions);
        }

        protected override TransitionTable TransitionTable
        {
            get { return this.transitionTable; }
        }

        protected override ILexeme CreateLexeme(String lexemeString)
        {
            return new MultiplicationLexeme();
        }
    }

    public class DivisionLexemeAnalyser : LexemeAnalyserBase
    {
        private readonly TransitionTable transitionTable;

        public DivisionLexemeAnalyser()
        {
            var transitions = new Transition[]
            {
                new Transition(0, 1, SlashCharCategory.Get(), TransitionKind.Straight)
            };

            this.transitionTable = new TransitionTable(2, transitions);
        }

        protected override TransitionTable TransitionTable
        {
            get { return this.transitionTable; }
        }

        protected override ILexeme CreateLexeme(String lexemeString)
        {
            return new DivisionLexeme();
        }
    }

    public class OpeningBracketLexemeAnalyser : LexemeAnalyserBase
    {
        private readonly TransitionTable transitionTable;

        public OpeningBracketLexemeAnalyser()
        {
            var transitions = new Transition[]
            {
                new Transition(0, 1, OpeningBracketCharCategory.Get(), TransitionKind.Straight)
            };

            this.transitionTable = new TransitionTable(2, transitions);
        }

        protected override TransitionTable TransitionTable
        {
            get { return this.transitionTable; }
        }

        protected override ILexeme CreateLexeme(String lexemeString)
        {
            return new OpeningBracketLexeme();
        }
    }

    public class ClosingBracketLexemeAnalyser : LexemeAnalyserBase
    {
        private readonly TransitionTable transitionTable;

        public ClosingBracketLexemeAnalyser()
        {
            var transitions = new Transition[]
            {
                new Transition(0, 1, ClosingBracketCharCategory.Get(), TransitionKind.Straight)
            };

            this.transitionTable = new TransitionTable(2, transitions);
        }

        protected override TransitionTable TransitionTable
        {
            get { return this.transitionTable; }
        }

        protected override ILexeme CreateLexeme(String lexemeString)
        {
            return new ClosingBracketLexeme();
        }
    }
}
