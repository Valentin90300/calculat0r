using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task
{
    public class DefaultPriorityCategoryRegister : PriorityCategoryRegisterBase
    {
        protected override void SetupPriorityCategoryList()
        {
            var bolp = BinaryOperatorLowPriorityCategory.Get();
            bolp.SetPriorityCategoryRegister(this);
            this.Add(bolp);

            var bohp = BinaryOperatorHighPriorityCategory.Get();
            bohp.SetPriorityCategoryRegister(this);
            this.Add(bohp);

            var uonp = UnaryOperatorNormalPriorityCategory.Get();
            uonp.SetPriorityCategoryRegister(this);
            this.Add(uonp);
        }
    }
}
