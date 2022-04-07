using System;
using Xunit;

namespace Assignment1.Tests
{
    public class CalculatorTest
    {
        [Fact]
        public void TestResult()
        {
            // Arrange
            var equation = "55.80+10.1*80/4-10+2.5";
            var expected = 55.80 + 10.1 * 80 / 4 - 10 + 2.5; // 250,3
            // Act
            var result = Calculator.RunEquation(equation);
            // Assert
            Assert.Equal(expected.ToString(), result.ToString());
        }

        [Fact]
        public void TestFaultyInput()
        {
            // Arrange
            var equation = "10ö0+50";
            // Act
            var result = Calculator.RunEquation(equation);
            // Assert
            Assert.True(double.IsNaN(result));
        }

        [Fact]
        public void TestNull()
        {
            // Arrange
            // Act
            var result = Calculator.RunEquation(null);
            // Assert
            Assert.True(double.IsNaN(result));
        }

        [Fact]
        public void TestDevideByZero()
        {
            // Arrange
            // Act
            var result = Calculator.RunEquation("50+100/0");
            // Assert
            Assert.True(double.IsNaN(result));
            Assert.Equal("You were trying to devide by zero!", Calculator.GetLastError());
        }
    }

    public class MathTest
    {
        [Fact]
        public void TestDevideByZero()
        {
            // Arrange
            // Act
            var result = SimpleMath.Div(100d, 0d);
            // Assert
            Assert.True(double.IsNaN(result));
            Assert.Equal("You were trying to devide by zero!", SimpleMath.GetLastError());
        }

        [Fact]
        public void TestAddArray()
        {
            // Arrange
            var equation = new double[] { 10, 10, 10, 10, 10 };
            var expected = 10 + 10 + 10 + 10 + 10; // 50
            // Act
            var result = SimpleMath.Add(equation);
            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TestSubArray()
        {
            // Arrange
            var equation = new double[] { 10, 10, 10, 10, 10 };
            var expected = 10 - 10 - 10 - 10 - 10; // -30
            // Act
            var result = SimpleMath.Sub(equation);
            // Assert
            Assert.Equal(expected, result);
        }
    }
}
