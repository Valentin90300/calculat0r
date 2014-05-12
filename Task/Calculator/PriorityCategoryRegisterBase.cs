using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task
{
    public abstract class PriorityCategoryRegisterBase
    {
        private readonly List<IPriorityCategory> priorityCategoryList;

        public PriorityCategoryRegisterBase()
        {
            this.priorityCategoryList = new List<IPriorityCategory>();
            
            this.SetupPriorityCategoryList();
            if (this.priorityCategoryList.Count == 0)
            {
                throw new EmptyPriorityCategoryListException();
            }
        }

        public int GetPriorityNumber(IPriorityCategory priorityCategory)
        {
            if (priorityCategory == null)
            {
                throw new PriorityCategoryParameterNullException();
            }

            var index = this.priorityCategoryList.IndexOf(priorityCategory);
            if (index == -1)
            {
                throw new PriorityCategoryNotFoundException();
            }

            return index + 1;
        }

        protected void Add(IPriorityCategory priorityCategory)
        {
            if (priorityCategory == null)
            {
                throw new PriorityCategoryParameterNullException();
            }

            if 
            (
                this.priorityCategoryList.Contains(priorityCategory) ||
                this.priorityCategoryList.Select(pc => pc.GetType()).Contains(priorityCategory.GetType())
            )
            {
                throw new PriorityCategoryAlreadyAddedException();
            }

            this.priorityCategoryList.Add(priorityCategory);
        }

        protected abstract void SetupPriorityCategoryList();
    }
}