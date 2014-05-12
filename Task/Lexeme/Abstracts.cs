using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task
{
    public abstract class NumberLexemeBase : ILexeme
    {
        private readonly Object packedNumber;

        public LexemeKind LexemeKind
        {
            get { return LexemeKind.WithVariableLength; }
        }

        public Object PackedNumber
        {
            get { return this.packedNumber; }
        }

        public NumberLexemeBase(Object packedNumber)
        {
            if (packedNumber == null)
            {
                throw new ArgumentNullException();
            }
            if (packedNumber.GetType() != typeof(Int32) && packedNumber.GetType() != typeof(Double))
            {
                throw new ArgumentException();
            }
            this.packedNumber = packedNumber;
        }
    }

    public abstract class OperatorLexemeBase : ILexeme
    {
        private readonly IPriorityCategory priorityCategory;

        public LexemeKind LexemeKind
        {
            get { return LexemeKind.WithFixedLength; }
        }

        public IPriorityCategory PriorityCategory
        {
            get { return this.priorityCategory; }
        }

        public OperatorLexemeBase(IPriorityCategory priorityCategory)
        {
            this.priorityCategory = priorityCategory;
        }
    }

    public abstract class UnaryOperatorLexemeBase : OperatorLexemeBase
    {
        public UnaryOperatorLexemeBase(IPriorityCategory priorityCategory)
            :
            base(priorityCategory)
        {
        }

        public abstract NumberLexemeBase Perform(NumberLexemeBase operand);
    }

    public abstract class BinaryOperatorLexemeBase : OperatorLexemeBase
    {
        public BinaryOperatorLexemeBase(IPriorityCategory priorityCategory)
            :
            base(priorityCategory)
        {
        }

        public abstract NumberLexemeBase Perform(NumberLexemeBase operandA, NumberLexemeBase operandB);
    }
}
