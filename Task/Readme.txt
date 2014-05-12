Замечание №1. Использование механизма расширения списка поддерживаемых операторов:

1. (Необязательно) Создать для приоритета нового оператора подкласс класса PriorityCategoryBase,
   затем создать подкласс PriorityCategoryRegisterBase или DefaultPriorityCategoryRegister, в нем
   переопределить метод SetupPriorityCategoryList, задав в нем новый порядок приоритетов с учетом 
   нового. Изменить привязку для абстрактного класса PriorityCategoryRegisterBase в DIContainer.It.
2. Создать для нового оператора подкласс класса UnaryOperatorLexemeBase или BinaryOperatorLexemeBase,
   реализовав в нем метод Perform.
3. Создать для парсера нового оператора подкласс класса LexemeAnalyserBase, реализовав в нем абстрактные
   свойство TransitionTable и метод CreateLexeme.
4. Создать подкласс LexemeAnalyserSetBase или DefaultLexemeAnalyserSet, в нем переопределить метод 
   SetupLexemeAnalyserSet, добавив в нем через метод Add объект нового класса парсера оператора.
   Изменить привязку для абстрактного класса LexemeAnalyserSetBase в DIContainer.It.

Замечание №2. В качестве символа - разделителя целой и дробной частей вещественного числа используется
точка.

Замечание №3. В методе Add в классе LexemeAnalyserSetBase есть некоторое дополнительно проверочное условие, 
которое приводит к ошибкам в некоторых юнит-тестах и включается в компиляцию только при сборке в режиме 
Release. Поэтому запускать тесты нужно для Debug-версии сборки Task.