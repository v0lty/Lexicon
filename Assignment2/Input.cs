using System;
using System.Text.RegularExpressions;

using static System.Console;

namespace Assignment2
{
    public static class Input
    {
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

            return (input == ConsoleKey.Y);
        }

        public static string RequestInput(string message)
        {
            while (true) 
            {
                WriteLine(message);
                var input = ReadLine().ToUpper();

                if (Regex.Matches(input, @"[A-Za-z]").Count > 0)
                {
                    return input;
                }
                else
                    WriteLine("Accepted characters are A to Z, try again.");
            }
        }
    }
}
