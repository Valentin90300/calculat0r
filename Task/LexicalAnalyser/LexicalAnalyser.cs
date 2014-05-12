using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Ninject;

namespace Task
{
    internal class LexicalAnalyser : ILexicalAnalyser
    {
        private readonly LexemeAnalyserSetBase lexemeAnalyserSet;

        internal LexicalAnalyser()
            :
            this(DIContainer.It.Get<LexemeAnalyserSetBase>())
        {
        }

        internal LexicalAnalyser(LexemeAnalyserSetBase lexemeAnalyserSet)
        {
            this.lexemeAnalyserSet = lexemeAnalyserSet;
        }

        LinkedList<ILexeme> ILexicalAnalyser.Parse(BinaryReader expressionReader)
        {
            var lexemeLinkedList = new LinkedList<ILexeme>();

            while (expressionReader.PeekChar() != 0)
            {
                var lexeme = this.ParseLexeme
                (
                    expressionReader, lexemeLinkedList.Last == null ? null : lexemeLinkedList.Last.Value
                );

                if (lexeme.LexemeKind == LexemeKind.WithVariableLength)
                {
                    expressionReader.BaseStream.Position--;
                }

                lexemeLinkedList.AddLast(lexeme);
            }

            return lexemeLinkedList;
        }

        private ILexeme ParseLexeme(BinaryReader expressionReader, ILexeme previousLexeme)
        {
            ILexeme parsedLexeme = null;

            foreach (var p in this.lexemeAnalyserSet)
            {
                p.Reset();
            }

            while (true)
            {
                var streamEndReached = expressionReader.BaseStream.Position == expressionReader.BaseStream.Length;
                var atLeastOneLexemeAnalyserIsInProgress = this.lexemeAnalyserSet.Any
                (
                    p => p.Status == LexemeAnalyserStatus.InProgress
                );

                if (streamEndReached || !atLeastOneLexemeAnalyserIsInProgress)
                {
                    if (parsedLexeme == null)
                    {
                        throw new UnknownLexemeException();
                    }
                    else
                    {
                        return parsedLexeme;
                    }
                }

                this.Round(expressionReader, previousLexeme, ref parsedLexeme);
            }
        }

        private void Round(BinaryReader expressionReader, ILexeme previousLexeme, ref ILexeme parsedLexeme)
        {
            ILexeme parsedLexemeOnCurrentStep = null;

            var @char = expressionReader.ReadChar();

            var activeLexemeAnalyserSet = this.lexemeAnalyserSet.Where
            (
                p => p.Status == LexemeAnalyserStatus.InProgress
            )
            .ToArray();

            foreach (var p in activeLexemeAnalyserSet)
            {
                p.Step(@char);

                if (p.Status == LexemeAnalyserStatus.Completed)
                {
                    if (p.ParsedLexeme is OperatorLexemeBase)
                    {
                        if (!this.IsCorrectOperatorUsage((OperatorLexemeBase)p.ParsedLexeme, previousLexeme))
                        {
                            continue;
                        }
                    }

                    if (parsedLexemeOnCurrentStep == null)
                    {
                        parsedLexemeOnCurrentStep = p.ParsedLexeme;
                    }
                    else
                    {
                        throw new SimultaneousSeveralLexemeAnalysersCompletedException();
                    }
                }
            }

            if (parsedLexemeOnCurrentStep != null)
            {
                parsedLexeme = parsedLexemeOnCurrentStep;
            }
        }

        private Boolean IsCorrectOperatorUsage(OperatorLexemeBase operatorLexeme, ILexeme previousLexeme)
        {
            if (operatorLexeme is UnaryOperatorLexemeBase)
            {
                Boolean isCorrectUnaryOperatorUsage = previousLexeme == null || previousLexeme is OpeningBracketLexeme || 
                    previousLexeme is OperatorLexemeBase;

                return isCorrectUnaryOperatorUsage;
            }
            else
            {
                Boolean isCorrectBinaryOperatorUsage = previousLexeme is NumberLexemeBase || 
                    previousLexeme is ClosingBracketLexeme;

                return isCorrectBinaryOperatorUsage;
            }
        }
    }
}
