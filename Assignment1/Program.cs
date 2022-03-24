using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace Assignment1
{
    public static class SimpleMath
    {
        public static int Calculate(char operation, int a, int b)
        {
            switch (operation)
            {
                case '+': return Addition(a, b);
                case '-': return Subtraction(a, b);
                case '/': return Division(a, b);
                case '*': return Multiplication(a, b);
                default:
                    return 0;
            }
        }

        private static int Addition(int a, int b)
        {
            return a + b;
        }

        private static int Subtraction(int a, int b)
        {
            return a - b;
        }

        private static int Division(int a, int b)
        {
            // alternativt kan DivideByZeroException fångas i ett try block
            if (b == 0) {
                Console.WriteLine("You were trying to devide by zero!");
                return 0;
            }

            return a / b;
        }

        private static int Multiplication(int a, int b)
        {
            return a * b;
        }
    }

    public static class Calculator
    {
        public static readonly char[] Operators = new char[] { '*', '/', '-', '+' };

        public static int RunEquation(string input)
        {
            // Regex.Split(input, @"([\+\-\*\/\(\)])") ger samma resultat men
            // eftersom att det är en skoluppgift utförs det manuellt
            var list = SplitNumbersAndOperators(input, Operators);

            // ekvationer måste utföras i ordningen */-+ när flera tal används i följd
            for (int i = 0; i < Operators.Length; i++)
            {
                while (true) 
                {
                    // hitta första operatorn så ekvation kan utföras med talen till vänster och höger om den
                    int j = list.IndexOf(Operators[i].ToString());
                    if (j < 0)
                        break;

                    if (j + 1 >= list.Count) {
                        Console.WriteLine("Invalid format. Please check values and operators!");
                        return 0;
                    }

                    if (!int.TryParse(list[j - 1], out int valA)
                     || !int.TryParse(list[j + 1], out int valB)) {
                        Console.WriteLine("Invalid format. One or more values couldn't be parsed!");
                        return 0;
                    }

                    var equation = SimpleMath.Calculate(list[j][0], valA, valB);

                    // använda tal och operator raderas och ersätts med ekvationen och användas i nästa iteration
                    // tills att alla fält i listan tagits bort och enbart slutresultatet finns kvar.
                    list.RemoveRange(j - 1, 3);
                    list.Insert(j - 1, equation.ToString());
                }
            }

            if (list.Count == 0 || String.IsNullOrEmpty(list[0]))
            {
                Console.WriteLine("Equation failed!");
                return 0;
            }

            return int.Parse(list[0]);
        }

        private static List<string> SplitNumbersAndOperators(string input, char[] match)
        {
            var result = new List<string>(); 
            int oldPos = 0;
            int curPos = 0;
            
            do {
                // index av första påträffade operator
                curPos = input.IndexOfAny(match, oldPos);

                if (curPos >= 0) {
                    // lägg till operator samt talet innan
                    result.Add(input.Substring(oldPos, curPos - oldPos));
                    result.Add(input.Substring(curPos, 1));

                    oldPos = curPos + 1;
                }
            }
            while (curPos >= 0);
            // tal efter sista funna operatorn
            result.Add(input.Substring(oldPos, input.Length - oldPos));

            return result;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            do {
                Console.Clear();
                Console.WriteLine("Please enter your equation:");
                var input = Console.ReadLine(); // 55+10*80/3-10+2

                if (String.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Invalid format. Please enter some values!");
                }
                else if (input.Any(c => (!Char.IsDigit(c) && !Calculator.Operators.Contains<char>(c))))
                {
                    // varken nummer eller operator
                    Console.WriteLine("Invalid format. Equation contains invalid input!");
                }
                else {
                    // ta bort ev. blanksteg
                    input = new string(input.ToCharArray().Where(c => !Char.IsWhiteSpace(c)).ToArray());
                    var result = Calculator.RunEquation(input);

                    Console.WriteLine("\nResult: {0}", result);
                }

                Console.WriteLine("\nDo you want to exit? [Y/N]:");
            }
            while (Console.ReadKey().Key != ConsoleKey.Y);
        }
    }
}