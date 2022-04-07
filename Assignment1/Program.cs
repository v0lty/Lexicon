using System;

using static System.Console;

namespace Assignment1
{
    class Program
    {
        static void Main(string[] args)
        {
            do {
                Clear();
                WriteLine("Please enter your equation:");

                var input = ReadLine(); // 55+10*80/4-10+2
                var value = Calculator.RunEquation(input);

                while (double.IsNaN(value))
                {
                    WriteLine($"\nAn error occured: {Calculator.GetLastError()}\nTry again:");
                    input = ReadLine();
                    value = Calculator.RunEquation(input);
                }

                WriteLine($"\nResult: {value}");
            }
            while (!RequestConfirmation("Do you want to quit?"));
        }

        public static bool RequestConfirmation(string message)
        {
            ConsoleKey input;

            do {
                Write($"{message} [y/n]:");
                input = ReadKey(false).Key;

                if (input != ConsoleKey.Enter)
                    WriteLine();

            } while (input != ConsoleKey.Y 
                  && input != ConsoleKey.N);
            return  (input == ConsoleKey.Y);
        }
    }
}