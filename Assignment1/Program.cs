using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace Assignment1
{
    public static class SimpleMath
    {
        public static double Calculate(char operation, double a, double b)
        {
            switch (operation)
            {
                case '+': return Addition(a, b);
                case '-': return Subtraction(a, b);
                case '/': return Division(a, b);
                case '*': return Multiplication(a, b);
                default:
                    return 0d;
            }
        }

        private static double Addition(double a, double b)
        {
            return a + b;
        }

        private static double Subtraction(double a, double b)
        {
            return a - b;
        }

        private static double Division(double a, double b)
        {
            // alternativt kan DivideByZeroException fångas i ett try block
            if (b == 0d) {
                Console.WriteLine("You were trying to devide by zero!");
                return 0d;
            }

            return a / b;
        }

        private static double Multiplication(double a, double b)
        {
            return a * b;
        }
    }

    public static class Calculator
    {
        public static readonly char[] operators = new char[] { '*', '/', '-', '+' };

        public static double RunEquation(string input)
        {
            // Regex.Split(input, @"([\+\-\*\/\(\)])") ger samma resultat men
            // eftersom att det är en skoluppgift utförs det manuellt
            var inputList = SplitNumbersAndOperators(input, operators);

            // ekvationer måste utföras i ordningen */-+ när flera tal används i följd
            for (int i = 0; i < operators.Length; i++)
            {
                while (true) 
                {
                    // hitta första operatorn så ekvation kan utföras med talen till vänster och höger om den
                    int j = inputList.IndexOf(operators[i].ToString());
                    if (j < 0)
                        break;

                    if (j + 1 >= inputList.Count) {
                        Console.WriteLine("Invalid format. Please check values and operators!");
                        return 0d;
                    }

                    if (!double.TryParse(inputList[j - 1], out double valA)
                     || !double.TryParse(inputList[j + 1], out double valB)) {
                        Console.WriteLine("Invalid format. One or more values couldn't be parsed!");
                        return 0d;
                    }

                    var equation = SimpleMath.Calculate(inputList[j][0], valA, valB);

                    // använda tal och operator raderas och ersätts med ekvationen och användas i nästa iteration
                    // tills att alla fält i listan tagits bort och enbart slutresultatet finns kvar.
                    inputList.RemoveRange(j - 1, 3);
                    inputList.Insert(j - 1, equation.ToString());
                }
            }

            if (inputList.Count == 0 || String.IsNullOrEmpty(inputList[0]))
            {
                Console.WriteLine("Equation failed!");
                return 0d;
            }

            return double.TryParse(inputList[0], out double result) ? result : 0d;
        }

        private static List<string> SplitNumbersAndOperators(string input, char[] match)
        {
            var result = new List<string>(); 
            int oldPos = 0;

            while (true) {
                // index av första påträffade operator
                int curPos = input.IndexOfAny(match, oldPos);

                if (curPos >= 0) {
                    // lägg till operator samt talet innan
                    result.Add(input.Substring(oldPos, curPos - oldPos));
                    result.Add(input.Substring(curPos, 1));

                    oldPos = curPos + 1;
                }
                else {
                    break;
                }
            }

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
                var input = Console.ReadLine(); // 55+10*80/3-10+2 == 55+(((10*80)/3)-10)+2

                // ta bort ev. blanksteg
                input = new string(input.ToCharArray().Where(c => !Char.IsWhiteSpace(c)).ToArray());
                var result = Calculator.RunEquation(input);

                Console.WriteLine("\nResult: {0}", result);
                Console.WriteLine("\nDo you want to exit? [Y/N]:");
            }
            while (Console.ReadKey().Key != ConsoleKey.Y);
        }
    }
}