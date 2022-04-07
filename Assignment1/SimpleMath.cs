using System.Linq;

namespace Assignment1
{
    public static class SimpleMath
    {
        private static string lastError = null;

        public static double Calculate(char operation, double left, double right)
        {
            lastError = null;

            switch (operation)
            {
                case '+': return Add(new double[] { left, right }); // use overload
                case '-': return Sub(new double[] { left, right }); // use overload
                case '/': return Div(left, right);
                case '*': return Mul(left, right);
                default:
                    lastError = "You've entered an unsuported operator!";
                    return double.NaN;
            };
        }

        public static double Add(double left, double right)
        {
            return left + right;
        }

        /// <summary>
        /// Overloads Add(double, double, out double)
        /// </summary>
        public static double Add(double[] values)
        {
            return (values?.Length > 1) 
                ? values.Aggregate((temp, x) => temp + x) // foreach temp += x.. 
                : double.NaN;
        }

        public static double Sub(double left, double right)
        {
            return left - right;
        }

        /// <summary>
        /// Overloads Sub(double, double, out double)
        /// </summary>
        public static double Sub(double[] values)
        {
            return (values?.Length > 1) 
                ? values.Aggregate((temp, x) => temp - x) 
                : double.NaN;
        }

        public static double Div(double left, double right)
        {
            // reset
            lastError = null;

            if (right == 0d) {
                lastError = "You were trying to devide by zero!";
                return double.NaN;
            }

            return left / right;
        }

        public static double Mul(double left, double right)
        {
            return left * right;
        }

        public static string GetLastError()
        {
            return lastError;
        }
    }
}
