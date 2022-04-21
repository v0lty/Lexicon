using System;
using System.Collections.Generic;

using static System.Console;

namespace Assignment3
{
    class Program
    {
        static void Main(string[] args)
        {
            IVending vending = new VendingMachine();

            while (true) 
            {
                int itemIndex = 0;

                // print header
                Clear();
                WriteLine("Welcome to our Vending machine!");
                WriteLine("Current money: {0} kr.\n", vending.ShowCurrentMoney());
                WriteLine(new string('-', 80));
                WriteLine("INDEX | CATEGORY | PRODUCT | PRICE | STOCK | INFO");
                WriteLine(new string('-', 80));

                // print items
                foreach (var item in vending.ShowAll())
                {
                    ForegroundColor = (item.Value == 0) ? ConsoleColor.DarkRed : ConsoleColor.Gray; // out of stock == red

                    WriteLine("{0,5} | {1,-8} | {2,-7} | {3,5} | {4,5} | {5,-10}",
                       (itemIndex++).ToString(),                        
                        item.Key.GetType().Name,
                        item.Key.Name,
                        item.Key.Price,
                        item.Value,
                        item.Key.Examine()
                        );
                }

                ForegroundColor = ConsoleColor.Gray;
                WriteLine(new string('-', 80));

                // print options
                WriteLine();
                WriteLine("{0,5} | Insert money.", itemIndex + 1);
                WriteLine("{0,5} | End transaction.", itemIndex + 2);
                WriteLine($"\nOption [0-{itemIndex + 2}]:");

                // handle input
                int input;
                while (!int.TryParse(ReadLine(), out input)) {
                    Console.WriteLine("Invalid input, try again:");
                }

                if (input <= itemIndex) 
                {
                    Purchase(vending, input);
                    WriteLine("\nPress any key to continue..");
                    ReadKey();
                }
                else if (input == itemIndex + 1)
                {
                    InsertMoney(vending);
                    WriteLine("\nPress any key to continue..");
                    ReadKey();
                }
                else if (input == itemIndex + 2)
                {
                    EndTransaction(vending);
                    WriteLine("\nPress any key to quit..");
                    ReadKey();
                    Environment.Exit(0);
                }                
            }
        }

        private static void Purchase(IVending vending, int index)
        {
            Clear();
            var product = vending.Purchase(index, out string errorMessage);
            if (product != null)           
                WriteLine($"You bought one {product.Name}! Follow these instructions to use this product:\n{product.Use()}.");        
            else
                WriteLine($"Transaction rejected. Error: {errorMessage}.");          
        }

        private static void InsertMoney(IVending vending)
        {
            Clear();
            WriteLine("Enter amount:");

            if (vending.InsertMoney(int.Parse(ReadLine()), out string errorMessage))         
                WriteLine("Deposition accepted!");        
            else
                WriteLine($"Deposition rejected. Error: {errorMessage}.");

            WriteLine($"Money in machine: {vending.ShowCurrentMoney()}");
        }

        private static void EndTransaction(IVending vending)
        {            
            Clear();
            int total = vending.ShowCurrentMoney();

            vending.EndTransaction(out List<Product> products, out Dictionary<int, int> exchange);

            WriteLine("Money that you have deposited:");
            WriteLine($"{vending.ShowInsertedMoney()} kr.\n");

            WriteLine("Products that you bought:");
            foreach (var item in products)         
                WriteLine($"{item.Name} ({item.Price} kr).");                  

            WriteLine("\nYour exchange:");            
            foreach (var item in exchange)       
                WriteLine($"{item.Value} x {item.Key,4} kr.");
            
            WriteLine($"-------------");
            WriteLine($"{total} kr.\n");

        }
    }
}