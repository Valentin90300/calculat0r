using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task
{
    public abstract class PriorityCategoryBase<T> : IPriorityCategory where T : PriorityCategoryBase<T>, new()
    {
        #region Singleton infrastructure
        private static T instance = null;

        public static T Get()
        {
            if (instance == null) instance = new T();
            return instance;
        }

        protected PriorityCategoryBase()
        {
            if (instance != null)
            {
                throw new InvalidOperationException
                (
                    String.Format
                    (
                        "Не допускается создание более одного экземпляра класса {0}", typeof(T).Name
                    )
                );
            }
        }
        #endregion

        private PriorityCategoryRegisterBase register = null;

        public void SetPriorityCategoryRegister(PriorityCategoryRegisterBase register)
        {
            if (register == null)
            {
                throw new ArgumentNullException();
            }
            if (this.register != null)
            {
                throw new InvalidOperationException();
            }
            this.register = register;
        }

        public int GetPriorityNumber()
        {
            if (this.register == null)
            {
                throw new InvalidOperationException();
            }

            return this.register.GetPriorityNumber(instance);
        }
    }
}
