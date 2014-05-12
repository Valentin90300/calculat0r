using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Task;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var calculator = new Calculator();

                Console.WriteLine("Введите выражение:");
                var expressionString = Console.ReadLine();

                var result = calculator.Calculate(expressionString);

                var nfi = CultureInfo.InvariantCulture.NumberFormat;
                Console.WriteLine
                (
                    result is Int32 ? ((Int32)result).ToString(nfi) : ((Double)result).ToString(nfi)
                );
            }
            catch (Exception exc)
            {
                Console.WriteLine("В процессе работе программы произошла ошибка. Подробная информация:");
                Console.WriteLine(exc.Message);
            }

            Console.ReadKey(true);
        }
    }
}
