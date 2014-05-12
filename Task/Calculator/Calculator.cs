using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Ninject;

namespace Task
{
    public class Calculator : ICalculator
    {
        private readonly ILexicalAnalyser lexicalAnalyser;
        private readonly ISyntaxAnalyser syntaxAnalyser;
        private readonly PriorityCategoryRegisterBase priorityCategoryRegister;

        public Calculator()
            :
            this
            (
                new LexicalAnalyser(),
                new SyntaxAnalyser(),
                DIContainer.It.Get<PriorityCategoryRegisterBase>()
            )
        {
        }

        internal Calculator
        (
            ILexicalAnalyser lexicalAnalyser, 
            ISyntaxAnalyser syntaxAnalyser, 
            PriorityCategoryRegisterBase priorityCategoryRegister
        )
        {
            this.lexicalAnalyser = lexicalAnalyser;
            this.syntaxAnalyser = syntaxAnalyser;
            this.priorityCategoryRegister = priorityCategoryRegister;
        }

        public Object Calculate(String expressionString)
        {
            var expressionReader = this.ExpressionStringToExpressionReader(expressionString);

            var lexemeLinkedList = this.lexicalAnalyser.Parse(expressionReader);
            var expressionTree = this.syntaxAnalyser.Parse(lexemeLinkedList);

            var result = expressionTree.Calculate().PackedNumber;
            return result;
        }

        private BinaryReader ExpressionStringToExpressionReader(String expressionString)
        {
            if (expressionString == null)
            {
                throw new ExpressionStringNullException();
            }

            var filteredExpressionString = new String
            (
                expressionString.Where(@char => !Char.IsWhiteSpace(@char) && @char != '\0').ToArray()
            );

            if (filteredExpressionString == String.Empty)
            {
                throw new ExpressionStringDoesNotContainSignificantCharsException();
            }

            var preparedExpressionString = filteredExpressionString + '\0';
            var codedPreparedExpressionString = Encoding.UTF8.GetBytes(preparedExpressionString);
            var expressionMemoryStream = new MemoryStream(codedPreparedExpressionString);
            var expressionReader = new BinaryReader(expressionMemoryStream);
            
            return expressionReader;
        }
    }
}
