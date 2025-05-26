using FluentAssertions;

namespace CurrencyDotNet.UnitTests;

public class CurrencyTests
{

    [Theory]
    [InlineData("USD", "840", "United States dollar", "$", 2, null, null, null)]
    public void Currency_with_valid_params_should_be_created(
        string isoCode, string numericCode,
        string name, string symbol, int decimalPlaces, string? altName,
        string[]? locations, string? wikipediaUrl, string[]? alternativeSymbols = null)
    {
        // Act
        Currency currency = Currency.Create(
            isoCode, numericCode, name, symbol, decimalPlaces,
            altName, locations, wikipediaUrl, alternativeSymbols
        );

        // Assert
        currency.IsoCode.Should().Be(isoCode);
        currency.NumericCode.Should().Be(numericCode);
        currency.Name.Should().Be(name);
        currency.Symbol.Should().Be(symbol);
        currency.DecimalPlaces.Should().Be(decimalPlaces);
        currency.AltName.Should().Be(altName);
        currency.Locations.Should().BeEquivalentTo(locations);
        currency.WikipediaUrl.Should().Be(wikipediaUrl);
        currency.AlternativeSymbols.Should().BeEquivalentTo(alternativeSymbols);
        currency.Id.Should().NotBe(Guid.Empty);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(29)]
    public void Currency_cannot_be_created_with_invalid_decimal_count(int decimalCount)
    {
        Action act = () => Currency.Create(
            "IRR", "364", "Iranian Rial", "ریال", decimalCount
        );

        // Assert
        act.Should().Throw<ArgumentOutOfRangeException>();
    }
    
    [Theory]
    [InlineData("USD")]
    [InlineData("EUR")]
    [InlineData("GBP")]
    [InlineData("JPY")]
    [InlineData("IRR")]
    [InlineData("AED")]
    public void Curreny_with_isocode_should_return_correct_currency(string isoCode)
    {
        // Arrange
        var expectedCurrency = CurrencySource.FindByCode(isoCode);

        // Act
        var result = Currency.WithCode(isoCode);

        // Assert
        result.Should().BeEquivalentTo(expectedCurrency);
    }
    
}