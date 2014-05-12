using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Task
{
    #region Calculator class exceptions
    public class ExpressionStringNullException : ApplicationException
    {
        public ExpressionStringNullException()
            : base("Параметр expressionString имеет значение null.") { }
    }

    public class ExpressionStringDoesNotContainSignificantCharsException : ApplicationException
    {
        public ExpressionStringDoesNotContainSignificantCharsException()
            : base("Параметр expressionString не содержит значимых символов.") { }
    }
    #endregion

    #region LexicalAnalyser class exceptions
    public class UnknownLexemeException : ApplicationException
    {
        public UnknownLexemeException()
            : base("Обнаружен неизвестный тип лексемы либо допущена ошибка в конфигурации парсеров лексем.") { }
    }

    public class SimultaneousSeveralLexemeAnalysersCompletedException : ApplicationException
    {
        public SimultaneousSeveralLexemeAnalysersCompletedException()
            : base("Одной и той же последовательности символов соответствует более одной лексемы.") { }
    }
    #endregion

    #region LexemeAnalyserSetBase class exceptions
    public class EmptyLexemeAnalyserListException : ApplicationException
    {
        public EmptyLexemeAnalyserListException()
            : base("В класс LexemeAnalyserSetBase не было добавлено ни одного объекта ILexemeAnalyser.") { }
    }

    public class LexemeAnalyserParameterNullException : ApplicationException
    {
        public LexemeAnalyserParameterNullException()
            : base("Параметр lexemeAnalyser метода Add класса LexemeAnalyserSetBase равен null.") { }
    }

    public class LexemeAnalyserObjectAlreadyAddedException : ApplicationException
    {
        public LexemeAnalyserObjectAlreadyAddedException()
            : base("В класс LexemeAnalyserSetBase уже добавлен данный объект ILexemeAnalyser.") { }
    }

    public class LexemeAnalyserTypeAlreadyAddedException : ApplicationException
    {
        public LexemeAnalyserTypeAlreadyAddedException()
            : base("В класс LexemeAnalyserSetBase уже добавлен объект ILexemeAnalyser c таким же типом.") { }
    }
    #endregion

    #region LexemeAnalyserBase class exceptions
    public class NoAvailableParsedLexemeException : ApplicationException
    {
        public NoAvailableParsedLexemeException()
            : base("Считывание лексемы возможно только при переходе парсера в состояние Complited.") { }
    }

    public class InvalidAccessToStepMethodException : ApplicationException
    {
        public InvalidAccessToStepMethodException()
            : base("Вызов метода Step допустим только в состоянии парсера InProgress.") { }
    }

    public class MoreThanOneTransitionAvailableException : ApplicationException
    {
        public MoreThanOneTransitionAvailableException()
            : base("Текущему состоянию парсера и считанному символу соответствует более одного перехода.") { }
    }
    #endregion

    #region SyntaxAnalyser class exceptions
    public class ExpressionDoesNotContainOperandLexemeException : ApplicationException
    {
        public ExpressionDoesNotContainOperandLexemeException()
            : base("Выражение не содержит лексем-операндов.") { }
    }
    #endregion

    #region ExpressionAnalyser class exceptions
    public class EmptyExpressionException : ApplicationException
    {
        public EmptyExpressionException()
            : base("Обнаружено пустое выражение.") { }
    }

    public class OneOrMoreOpeningBracketMissingException : ApplicationException
    {
        public OneOrMoreOpeningBracketMissingException()
            : base("Пропущена одна или более открывающих скобок.") { }
    }

    public class OneOrMoreClosingBracketMissingException : ApplicationException
    {
        public OneOrMoreClosingBracketMissingException()
            : base("Пропущена одна или более закрывающих скобок.") { }
    }
    #endregion

    #region PriorityCategoryRegisterBase class exceptions
    public class EmptyPriorityCategoryListException : ApplicationException
    {
        public EmptyPriorityCategoryListException()
            : base("В класс PriorityCategoryRegisterBase не было добавлено ни одного объекта IPriorityCategory") { }
    }

    public class PriorityCategoryParameterNullException : ApplicationException
    {
        public PriorityCategoryParameterNullException()
            : base("Параметр priorityCategory равен null.") { }
    }

    public class PriorityCategoryNotFoundException : ApplicationException
    {
        public PriorityCategoryNotFoundException()
            : base("Данная категория приоритета не обнаружена.") { }
    }

    public class PriorityCategoryAlreadyAddedException : ApplicationException
    {
        public PriorityCategoryAlreadyAddedException()
            : base("Данная категория приоритета уже добавлена.") { }
    }
    #endregion

    #region Int32LexemeAnalyser class exceptions
    public class Int32StringParseException : ApplicationException
    {
        public Int32StringParseException()
            : base("При синтаксическом разборе строкового представления числа типа Int32 произошла ошибка.") { }
    }
    #endregion

    #region DoubleLexemeAnalyser class exceptions
    public class DoubleStringParseException : ApplicationException
    {
        public DoubleStringParseException()
            : base("При синтаксическом разборе строкового представления числа типа Double произошла ошибка.") { }
    }
    #endregion

    #region Lexeme classes exceptions
    public class AdditionLexemeException : ApplicationException
    {
        public AdditionLexemeException()
            : base("При выполнении операции сложения возникло переполнение.") { }
    }

    public class SubtractionLexemeException : ApplicationException
    {
        public SubtractionLexemeException()
            : base("При выполнении операции вычитания возникло переполнение.") { }
    }

    public class MultiplicationLexemeException : ApplicationException
    {
        public MultiplicationLexemeException()
            : base("При выполнении операции умножения возникло переполнение.") { }
    }

    public class DivisionLexemeException : ApplicationException
    {
        public DivisionLexemeException()
            : base("При выполнении операции деления возникло переполнение.") { }
    }
    #endregion
}
