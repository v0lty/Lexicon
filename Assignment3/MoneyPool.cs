using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Assignment3
{
    public class MoneyPool
    {
        private static readonly int[] currency = new int[] { 1, 5, 10, 20, 50, 100, 500, 1000 };

        public int Value { get; private set; }
        public int Total { get; private set; } // total amount inserted..

        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public int Deposit(int value, bool validate = true)
        {
            if (!validate || IsValidValue(value))
            {
                if (value > 0)
                    Total += value;

                return this.Value += value;
            }
            else
                throw new ArgumentOutOfRangeException($"Invalid input, allowed values are: " +
                    $"{string.Join(", ", currency.Select(s => s.ToString()).ToArray())} kronor");
        }

        public Dictionary<int, int> Whitdraw()
        {
            var result = new Dictionary<int, int>();
                        
            for (int i = currency.Length - 1; i >= 0; i--) // step backwards
            {
                while (Value >= currency[i])
                {
                    if (result.TryAdd(currency[i], 1) == false)
                        // key already exists -> increase amount
                        result[currency[i]] += 1;

                    Value -= currency[i];
                }
            }

            return result;
        }

        private bool IsValidValue(int value)
        {
            return Array.Exists(currency, element => element == value);
        }
    }
}