using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Assignment3
{
    public interface IVending
    {
        public Product Purchase(int index, out string errorMessage);
        public Dictionary<Product, int> ShowAll();
        public bool InsertMoney(int value, out string errorMessage);
        public int ShowInsertedMoney();
        public int ShowCurrentMoney();        
        public void EndTransaction(out List<Product> products, out Dictionary<int, int> exchange);
    }

    public class VendingMachine : IVending
    {
        public MoneyPool Money { get; private set; }
        public Dictionary<Product, int> Products { get; private set; }
        public List<Product> Purchases { get; private set; }

        public VendingMachine()
        {
            Money = new MoneyPool();
            Products = new Dictionary<Product, int>();
            Purchases = new List<Product>();
            var random = new Random();

            Products.Add(new Fruit("Apple", 10, true), 9);
            Products.Add(new Fruit("Banana", 10, false), random.Next(0, 10));
            Products.Add(new Fruit("Orange", 10, false), random.Next(0, 10));
            Products.Add(new Drink("Coke", 15, 33), random.Next(0, 10));
            Products.Add(new Drink("Coke", 20, 50), random.Next(0, 10));
            Products.Add(new Drink("Fanta", 15, 33), random.Next(0, 10));
            Products.Add(new Drink("Fanta", 20, 50), random.Next(0, 10));
            Products.Add(new Drink("Sprite", 15, 33), random.Next(0, 10));
            Products.Add(new Snack("Lion", 5, 1), random.Next(0, 10));
            Products.Add(new Snack("Lion", 10,2), random.Next(0, 10));
            Products.Add(new Snack("Mars", 5, 1), random.Next(0, 10));
            Products.Add(new Snack("Daim", 5, 1), random.Next(0, 10));
            Products.Add(new Snack("Daim", 10, 2), random.Next(0, 10));

            // sort by category
            Products = Products.OrderBy(x => x.Key.GetType().Name).ToDictionary(x => x.Key, x => x.Value);
        }

        public Product Purchase(int index, out string errorMessage)
        {
            if (index < 0 || index > Products.Count) {
                errorMessage = "Invalid index";
                return null;
            }

            var stockAmount = Products.ElementAt(index).Value;

            if (stockAmount == 0) {
                errorMessage = "Product is currently out of stock";
                return null;
            }

            var product = Products.ElementAt(index).Key;

            if (product.Price > Money.Value) {
                errorMessage = "Insufficient funds";
                return null;
            }

            Money.Deposit(-product.Price, validate: false);
            Products[product] = stockAmount - 1; // decrease stock
            Purchases.Add(product);
            errorMessage = null;
            return product;
        }

        public Dictionary<Product, int> ShowAll()
        {
            return Products;
        }

        public bool InsertMoney(int value, out string errorMessage)
        {
            try {
                Money.Deposit(value);
                errorMessage = null;
                return true;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                errorMessage = ex.ParamName;
                return false;
            }
        }

        public int ShowCurrentMoney()
        {
            return Money.Value;
        }

        public int ShowInsertedMoney()
        {
            return Money.Total;
        }

        public void EndTransaction(out List<Product> products, out Dictionary<int, int> exchange)
        {            
            products = Purchases;
            exchange = Money.Whitdraw();
        }
    }
}