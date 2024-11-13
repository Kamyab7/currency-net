using CurrencyDotNet;
using FluentAssertions;

namespace CurrencyDotNet.UnitTests;

public class CurrencyTests
{

    [Theory]
    [InlineData(-1)]
    [InlineData(29)]
    public void Currency_cannot_be_created_with_invalid_decimal_count(int decimalCount)
    {
        Action act = () => Currency.Create(
            "IRR", "364", "Iranian Rial", "ریال", decimalCount
        );

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*decimalCount's value cannot be less than zero or greater than 28.*");
    }
    
}