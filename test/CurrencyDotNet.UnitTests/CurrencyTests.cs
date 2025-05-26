using Shouldly;

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
        currency.IsoCode.ShouldBe(isoCode);
        currency.NumericCode.ShouldBe(numericCode);
        currency.Name.ShouldBe(name);
        currency.Symbol.ShouldBe(symbol);
        currency.DecimalPlaces.ShouldBe(decimalPlaces);
        currency.AltName.ShouldBe(altName);
        currency.Locations.ShouldBe(locations);
        currency.WikipediaUrl.ShouldBe(wikipediaUrl);
        currency.AlternativeSymbols.ShouldBe(alternativeSymbols);
        currency.Id.ShouldNotBe(Guid.Empty);
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
        act.ShouldThrow<ArgumentOutOfRangeException>();
    }
    
    [Theory]
    [InlineData("USD")][InlineData("EUR")][InlineData("GBP")][InlineData("JPY")]
    [InlineData("IRR")][InlineData("AED")]
    public void Curreny_with_isocode_should_return_correct_currency(string isoCode)
    {
        // Arrange
        var expectedCurrency = CurrencySource.FindByCode(isoCode);

        // Act
        var result = Currency.WithCode(isoCode);

        // Assert
        result.ShouldBeEquivalentTo(expectedCurrency);
    }
    
}