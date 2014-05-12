using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task
{
    internal interface IExpressionLexeme : ILexeme, IEnumerable<ILexeme>
    {
        void SplitExpressionByBinaryOperatorWithLowestPriority
        (
            out BinaryOperatorLexemeBase binaryOperatorLexeme,
            out IExpressionLexeme leftExpressionLexeme,
            out IExpressionLexeme rightExpressionLexeme
        );

        UnaryOperatorLexemeBase TakeUnaryOperatorWithLowestPriority();
    }
}
