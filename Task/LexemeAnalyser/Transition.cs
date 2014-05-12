using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task
{
    public class Transition
    {
        private readonly Int32 stateFromIndex;
        private readonly Int32 stateToIndex;
        private readonly ICharCategory charCategory;
        private readonly TransitionKind transitionKind;

        public Int32 StateFromIndex
        {
            get { return this.stateFromIndex; }
        }

        public Int32 StateToIndex
        {
            get { return this.stateToIndex; }
        }

        public ICharCategory CharCategory
        {
            get { return this.charCategory; }
        }

        public TransitionKind TransitionKind
        {
            get { return this.transitionKind; }
        }

        public Transition
        (
            Int32 stateFromIndex, Int32 stateToIndex, ICharCategory charCategory, TransitionKind transitionKind
        )
        {
            if (stateFromIndex < 0 || stateToIndex < 0)
            {
                throw new ArgumentException();
            }

            if (charCategory == null)
            {
                throw new ArgumentNullException();
            }

            this.stateFromIndex = stateFromIndex;
            this.stateToIndex = stateToIndex;
            this.charCategory = charCategory;
            this.transitionKind = transitionKind;
        }
    }
}
