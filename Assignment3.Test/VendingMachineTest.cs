using System;
using System.Collections.Generic;
using System.Linq;

using Xunit;

namespace Assignment3.Test
{
    public class VendingMachineTest
    {
        [Fact]
        public void TestInsertMoney()
        {
            // Arrange
            IVending vending = new VendingMachine();
            // Act
            bool result0 = vending.InsertMoney(999, out _); // invalid
            bool result1 = vending.InsertMoney(500, out _);
            bool result2 = vending.InsertMoney(100, out _);
            // Assert
            Assert.True(result0 == false); 
            Assert.True(result1); 
            Assert.True(result2); 
            Assert.Equal(600, vending.ShowCurrentMoney()); // 500 + 100 == 600
        }

        [Fact]
        public void TestShowProducts()
        {
            // Arrange
            IVending vending = new VendingMachine();
            // Act
            var products = vending.ShowAll();
            // Assert    
            Assert.NotNull(products);
            Assert.True(products.Count > 0);
        }

        [Fact]
        public void TestPurchase()
        {
            // Arrange
            IVending vending = new VendingMachine();
            // Act
            vending.InsertMoney(500, out _);
            var products = vending.ShowAll();
            var product = products.First(item => item.Value > 0).Key; // first avliable product with more then 0 in stock 
            var result = vending.Purchase(Array.IndexOf(products.Keys.ToArray(), product), out _);
            // Assert
            Assert.Equal(product, result);
            Assert.Equal(500, vending.ShowInsertedMoney()); 
            Assert.Equal(500 - product.Price, vending.ShowCurrentMoney()); 
        }

        [Fact]
        public void TestEndTransaction()
        {
            // Arrange
            IVending vending = new VendingMachine();
            // Act
            vending.InsertMoney(500, out _);
            var products = vending.ShowAll();
            var product = products.First(item => item.Value > 0).Key;
            var result = vending.Purchase(Array.IndexOf(products.Keys.ToArray(), product), out _);
            vending.EndTransaction(out List<Product> purchases, out Dictionary<int, int> exchange);
            // Assert
            Assert.Contains(result, purchases);
            Assert.Contains(100, exchange.Keys);
            Assert.Equal(4, exchange[100]); // 4 x 100 + ...
            Assert.Contains(50, exchange.Keys);
            Assert.Equal(1, exchange[50]);  // 4 x 100 + 1 x 50 ...
        }
    }
}
