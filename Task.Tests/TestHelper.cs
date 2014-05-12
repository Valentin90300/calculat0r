using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Task.Tests
{
    public static class TestHelper
    {
        public static void ExceptionThrowingCheck<TException>(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            Exception catchedException = null;
            try
            {
                action.Invoke();
            }
            catch (Exception exc)
            {
                catchedException = exc;
            }

            Assert.IsNotNull(catchedException);
            Assert.IsInstanceOfType(catchedException, typeof(TException));
        }
    }
}
