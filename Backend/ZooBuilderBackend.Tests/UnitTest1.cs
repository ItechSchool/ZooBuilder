using Xunit;

public class Calculator
{
    public int Add(int a, int b)
    {
        return a + b;
    }
}

public class CalculatorTests
{
    [Fact]
    public void Add_ShouldReturnSumOfTwoNumbers()
    {
        // Arrange
        var calculator = new Calculator();
        
        // Act
        int result = calculator.Add(2, 3);

        // Assert
        Assert.Equal(4, result);
    }
}