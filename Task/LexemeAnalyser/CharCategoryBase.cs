using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task
{
    public abstract class CharCategoryBase<T> : ICharCategory where T : CharCategoryBase<T>, new()
    {
        private static T instance;

        public static T Get()
        {
            if (instance == null)
            {
                instance = new T();
            }

            return instance;
        }

        protected CharCategoryBase()
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

        public virtual Boolean Contains(Char @char)
        {
            throw new NotImplementedException("");
        }
    }
}
