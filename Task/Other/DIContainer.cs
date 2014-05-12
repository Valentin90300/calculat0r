using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;

namespace Task
{
    public static class DIContainer
    {
        public static IKernel It { get; set; }

        static DIContainer()
        {
            It = new StandardKernel();
            DefaultSetup();
        }

        private static void DefaultSetup()
        {
            It.Bind<LexemeAnalyserSetBase>().To<DefaultLexemeAnalyserSet>();
            It.Bind<PriorityCategoryRegisterBase>().To<DefaultPriorityCategoryRegister>();
        }
    }
}
