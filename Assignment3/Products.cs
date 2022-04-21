using System;
using System.Collections.Generic;
using System.Text;

namespace Assignment3
{
    public abstract class Product
    {
        public virtual string Name { get; protected set; } 
        public virtual int Price { get; protected set; } 

        public abstract string Examine(); 
        public abstract string Use(); 

        public Product(string name, int price)
        {
            Name = name;
            Price = price;
        }
    }

    public class Drink : Product
    {        
        public virtual int Volume { get; protected set; }

        public Drink(string name, int price, int volume) : base(name, price)
        {
            Volume = volume;
        }

        public override string Examine()
        {
            return string.Format($"{(Volume > 33 ? "Large" : "Small")} cold drink containing {Volume} cl.");
        }

        public override string Use()
        {
            return $"Pop the can ta access the {Volume} cl of your drink";
        }
    }

    public class Snack : Product
    {
        public virtual int Pieces { get; protected set; }

        public Snack(string name, int price, int pieces) : base(name, price)
        {
            Pieces = pieces;
        }

        public override string Examine()
        {
            return string.Format($"Chocolate bar containing {Pieces} piece{(Pieces == 1 ? "" : "s")}.");
        }

        public override string Use()
        {
            return Pieces == 1 
                ? $"Unwrap the cover to access the chocolate" 
                : $"Unwrap the cover to access the {Pieces} pieces of chocolate";
        }
    }

    public class Fruit : Product
    {
        public virtual bool EdiblelePeel { get; protected set; }

        public Fruit(string name, int price, bool ediblelePeel) : base(name, price)
        {
            EdiblelePeel = ediblelePeel;
        }

        public override string Examine()
        {
            return "Fresh fruit.";
        }

        public override string Use()
        {
            return EdiblelePeel
                ? "The fruit peel is edible - just have a bite"
                : "Unpeel the fruit before you have a bite";
        }
    }
}