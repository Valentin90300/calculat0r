using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task
{
    public class UnaryOperatorNormalPriorityCategory 
        : 
        PriorityCategoryBase<UnaryOperatorNormalPriorityCategory>
    {
    }

    public class BinaryOperatorLowPriorityCategory 
        : 
        PriorityCategoryBase<BinaryOperatorLowPriorityCategory>
    {
    }

    public class BinaryOperatorHighPriorityCategory 
        : 
        PriorityCategoryBase<BinaryOperatorHighPriorityCategory>
    {
    }
}
