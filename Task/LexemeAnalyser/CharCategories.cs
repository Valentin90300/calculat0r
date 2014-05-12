using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task
{
    public class DecimalDigitCharCategory : CharCategoryBase<DecimalDigitCharCategory>
    {
        public override Boolean Contains(Char @char)
        {
            return Char.IsDigit(@char);
        }
    }

    public class SignCharCategory : CharCategoryBase<SignCharCategory>
    {
        public override Boolean Contains(Char @char)
        {
            return @char == '+' || @char == '-';
        }
    }

    public class DotCharCategory : CharCategoryBase<DotCharCategory>
    {
        public override Boolean Contains(Char @char)
        {
            return @char == '.';
        }
    }

    public class PlusCharCategory : CharCategoryBase<PlusCharCategory>
    {
        public override Boolean Contains(Char @char)
        {
            return @char == '+';
        }
    }

    public class MinusCharCategory : CharCategoryBase<MinusCharCategory>
    {
        public override Boolean Contains(Char @char)
        {
            return @char == '-';
        }
    }

    public class AsteriskCharCategory : CharCategoryBase<AsteriskCharCategory>
    {
        public override Boolean Contains(Char @char)
        {
            return @char == '*';
        }
    }

    public class SlashCharCategory : CharCategoryBase<SlashCharCategory>
    {
        public override Boolean Contains(Char @char)
        {
            return @char == '/';
        }
    }

    public class OpeningBracketCharCategory : CharCategoryBase<OpeningBracketCharCategory>
    {
        public override Boolean Contains(Char @char)
        {
            return @char == '(';
        }
    }

    public class ClosingBracketCharCategory : CharCategoryBase<ClosingBracketCharCategory>
    {
        public override Boolean Contains(Char @char)
        {
            return @char == ')';
        }
    }
}
