using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Assignment1
{
    public static class Calculator
    {
        private static readonly char[] operators = new char[] { '*', '/', '-', '+' };
        private static string lastError = null;

        public static double RunEquation(string input)
        {
            lastError = null;

            if (!IsValidInput(input)) {
                lastError = "Invalid input!\nAllowed syntax: <int or decimal><'+'or'-'or'*'or'/'><int or decimal>";
                return double.NaN;
            }

            var inputList = SplitInput(TrimInput(input), operators);

            // equation requires operators to be calculated in correct order (*/-+) 
            // 55+10*80/4-10+2 is therefor equal to 55+(((10*80)/4)-10)+2 == 247
            for (int i = 0, j; i < operators.Length; i++)
            {
                // loop equation for every instance current operator 
                while ((j = inputList.IndexOf(operators[i].ToString())) >= 0)
                {
                    // run equation of values left and right of found operator
                    if (!double.TryParse(inputList[j - 1], out double valueLeft)
                     || !double.TryParse(inputList[j + 1], out double valueRight)) {
                        lastError = "One or more values could not be parsed!";
                        return double.NaN;
                    }

                    var equation = SimpleMath.Calculate(operators[i], valueLeft, valueRight);

                    if (double.IsNaN(equation)) {
                        lastError = SimpleMath.GetLastError();
                        return double.NaN;
                    }
                    
                    // replace values and operator with the result so it can be used in next iteration
                    inputList.RemoveRange(j - 1, 3);
                    inputList.Insert(j - 1, equation.ToString());
                }
            }

            if (inputList == null || inputList.Count == 0) {
                Console.WriteLine("Unknown Error");
                return double.NaN;
            }

            // only inputList[0] will remain when all equations have been done
            return double.TryParse(inputList[0], out double result) ? result : double.NaN;
        }

        private static bool IsValidInput(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;

            // only allow unlimited chunks of <int or decimal><'+'or'-'or'*'or'/'><int or decimal>
            return Regex.IsMatch(input.Replace(',', '.'), 
                @"^(([0-9]+\.?[0-9]*|\.[0-9]+))(\s*[-+*/]\s*([0-9]+\.?[0-9]*|\.[0-9]+))+$");
        }

        private static string TrimInput(string input)
        {
            // allow both '.' and ',' as delimiter
            var temp = input.Replace('.', ',');
            // remove all white spaces
            return new string(temp.ToCharArray().Where(c => !char.IsWhiteSpace(c)).ToArray());
        }

        private static List<string> SplitInput(string input, char[] pattern)
        {
            var result = new List<string>(); 
            int oldPos = 0;
            int curPos = 0;

            // find every instance of any operator
            while ((curPos = input.IndexOfAny(pattern, oldPos)) >= 0)
            {
                // value left of current operator / right of last operator
                result.Add(input[oldPos..(curPos)]);
                // current operator
                result.Add(input[curPos..(curPos + 1)]); 
                oldPos = curPos + 1; 
            }

            result.Add(input[oldPos..]); 
            return result;
        }

        public static string GetLastError()
        {
            return lastError;
        }
    }
}
