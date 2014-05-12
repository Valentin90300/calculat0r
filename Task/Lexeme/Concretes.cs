using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task
{
    public class Int32Lexeme : NumberLexemeBase
    {
        public Int32Lexeme(Int32 int32value)
            :
            base(int32value)
        {
        }
    }

    public class DoubleLexeme : NumberLexemeBase
    {
        public DoubleLexeme(Double doubleValue)
            :
            base(doubleValue)
        {
        }
    }

    public class UnaryPlusLexeme : UnaryOperatorLexemeBase
    {
        public UnaryPlusLexeme()
            :
            base(UnaryOperatorNormalPriorityCategory.Get())
        {
        }

        public override NumberLexemeBase Perform(NumberLexemeBase operand)
        {
            if (operand.PackedNumber is Int32)
            {
                return new Int32Lexeme
                (
                    (Int32)operand.PackedNumber
                );
            }
            else
            {
                return new DoubleLexeme
                (
                    (Double)operand.PackedNumber
                );
            }
        }
    }

    public class UnaryMinusLexeme : UnaryOperatorLexemeBase
    {
        public UnaryMinusLexeme()
            :
            base(UnaryOperatorNormalPriorityCategory.Get())
        {
        }

        public override NumberLexemeBase Perform(NumberLexemeBase operand)
        {
            if (operand.PackedNumber is Int32)
            {
                return new Int32Lexeme
                (
                    - (Int32)operand.PackedNumber
                );
            }
            else
            {
                return new DoubleLexeme
                (
                    - (Double)operand.PackedNumber
                );
            }
        }
    }

    public class AdditionLexeme : BinaryOperatorLexemeBase
    {
        public AdditionLexeme()
            :
            base(BinaryOperatorLowPriorityCategory.Get())
        {
        }

        public override NumberLexemeBase Perform(NumberLexemeBase operandA, NumberLexemeBase operandB)
        {
            try
            {
                var a = operandA.PackedNumber;
                var b = operandB.PackedNumber;

                if (a.GetType() == b.GetType())
                {
                    if (a is Int32)
                    {
                        return new Int32Lexeme(checked((Int32)a + (Int32)b));
                    }
                    else
                    {
                        return new DoubleLexeme(checked((Double)a + (Double)b));
                    }
                }
                else
                {
                    if (a is Int32)
                    {
                        return new DoubleLexeme(checked((Int32)a + (Double)b));
                    }
                    else
                    {
                        return new DoubleLexeme(checked((Double)a + (Int32)b));
                    }
                }
            }
            catch (OverflowException)
            {
                throw new AdditionLexemeException();
            }
        }
    }

    public class SubtractionLexeme : BinaryOperatorLexemeBase
    {
        public SubtractionLexeme()
            :
            base(BinaryOperatorLowPriorityCategory.Get())
        {
        }

        public override NumberLexemeBase Perform(NumberLexemeBase operandA, NumberLexemeBase operandB)
        {
            try
            {
                var a = operandA.PackedNumber;
                var b = operandB.PackedNumber;

                if (a.GetType() == b.GetType())
                {
                    if (a is Int32)
                    {
                        return new Int32Lexeme(checked((Int32)a - (Int32)b));
                    }
                    else
                    {
                        return new DoubleLexeme(checked((Double)a - (Double)b));
                    }
                }
                else
                {
                    if (a is Int32)
                    {
                        return new DoubleLexeme(checked((Int32)a - (Double)b));
                    }
                    else
                    {
                        return new DoubleLexeme(checked((Double)a - (Int32)b));
                    }
                }
            }
            catch (OverflowException)
            {
                throw new AdditionLexemeException();
            }
        }
    }

    public class MultiplicationLexeme : BinaryOperatorLexemeBase
    {
        public MultiplicationLexeme()
            :
            base(BinaryOperatorHighPriorityCategory.Get())
        {
        }

        public override NumberLexemeBase Perform(NumberLexemeBase operandA, NumberLexemeBase operandB)
        {
            try
            {
                var a = operandA.PackedNumber;
                var b = operandB.PackedNumber;

                if (a.GetType() == b.GetType())
                {
                    if (a is Int32)
                    {
                        return new Int32Lexeme(checked((Int32)a * (Int32)b));
                    }
                    else
                    {
                        return new DoubleLexeme(checked((Double)a * (Double)b));
                    }
                }
                else
                {
                    if (a is Int32)
                    {
                        return new DoubleLexeme(checked((Int32)a * (Double)b));
                    }
                    else
                    {
                        return new DoubleLexeme(checked((Double)a * (Int32)b));
                    }
                }
            }
            catch (OverflowException)
            {
                throw new AdditionLexemeException();
            }
        }
    }

    public class DivisionLexeme : BinaryOperatorLexemeBase
    {
        public DivisionLexeme()
            :
            base(BinaryOperatorHighPriorityCategory.Get())
        {
        }

        public override NumberLexemeBase Perform(NumberLexemeBase operandA, NumberLexemeBase operandB)
        {
            try
            {
                var a = operandA.PackedNumber;
                var b = operandB.PackedNumber;

                if (a.GetType() == b.GetType())
                {
                    if (a is Int32)
                    {
                        return new Int32Lexeme(checked((Int32)a / (Int32)b));
                    }
                    else
                    {
                        return new DoubleLexeme(checked((Double)a / (Double)b));
                    }
                }
                else
                {
                    if (a is Int32)
                    {
                        return new DoubleLexeme(checked((Int32)a / (Double)b));
                    }
                    else
                    {
                        return new DoubleLexeme(checked((Double)a / (Int32)b));
                    }
                }
            }
            catch (OverflowException)
            {
                throw new AdditionLexemeException();
            }
        }
    }

    public class OpeningBracketLexeme : ILexeme
    {
        public LexemeKind LexemeKind
        {
            get { return LexemeKind.WithFixedLength; }
        }
    }

    public class ClosingBracketLexeme : ILexeme
    {
        public LexemeKind LexemeKind
        {
            get { return LexemeKind.WithFixedLength; }
        }
    }
}
