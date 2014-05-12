using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task
{
    public class TransitionTable
    {
        private readonly Int32 stateCount;
        private readonly ReadOnlyCollection<Transition> transitions;

        public Int32 StateCount
        {
            get { return this.stateCount; }
        }

        public ReadOnlyCollection<Transition> Transitions
        {
            get { return this.transitions; }
        }

        public TransitionTable(Int32 stateCount, IEnumerable<Transition> transitions)
        {
            if (stateCount < 2)
            {
                throw new ArgumentException();
            }

            if (transitions == null)
            {
                throw new ArgumentNullException();
            }

            if (!transitions.Any())
            {
                throw new ArgumentException();
            }

            foreach (var t in transitions)
            {
                if (t == null)
                {
                    throw new ArgumentNullException();
                }

                if (t.StateFromIndex >= stateCount)
                {
                    throw new ArgumentException();
                }

                if (t.StateToIndex >= stateCount)
                {
                    throw new ArgumentException();
                }
            }

            this.stateCount = stateCount;
            this.transitions = new ReadOnlyCollection<Transition>
            (
                (transitions as IList<Transition>) ?? new List<Transition>(transitions)
            );
        }
    }
}
