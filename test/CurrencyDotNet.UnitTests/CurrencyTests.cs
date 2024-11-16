using FluentAssertions;

namespace CurrencyDotNet.UnitTests;

public class CurrencyTests
{

    [Theory]
    [InlineData("USD", "840", "United States dollar", "$", 2, null, null, null)]
    public void Currency_with_valid_params_should_be_created(
        string isoCode, string numericCode,
        string name, string symbol, int decimalCount, string? altName,
        string[]? locations, string? wikipediaUrl, string[]? alternativeSymbols = null)
    {
        // Act
        Currency currency = Currency.Create(
            isoCode, numericCode, name, symbol, decimalCount,
            altName, locations, wikipediaUrl, alternativeSymbols
        );

        // Assert
        currency.IsoCode.Should().Be(isoCode);
        currency.NumericCode.Should().Be(numericCode);
        currency.Name.Should().Be(name);
        currency.Symbol.Should().Be(symbol);
        currency.DecimalCount.Should().Be(decimalCount);
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
        act.Should().Throw<ArgumentOutOfRangeException>()
            .WithMessage("*decimalCount's value cannot be less than zero or greater than 28.*");
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
        result.Should().BeEquivalentTo(expectedCurrency);
    }
    
    [Fact]
    public void Currency_props_should_be_immutable()
    {
        // Arrange
        var currency = Currency.Create("AUD", "036", "Australian Dollar", "$", 2, "دلار استرالیا",
            new[] { "Australia", "Kiribati", "Nauru" },
            "https://en.wikipedia.org/wiki/Australian_dollar",
            new string[] { });

        // Act & Assert
        // Attempting to modify properties should not be possible as setters are private.
        // This is implicitly tested by the compiler. For runtime checks, reflection can be used.

        // Example: Ensure that properties cannot be set via reflection (optional)
        Action act = () =>
        {
            var type = typeof(Currency);
            var prop = type.GetProperty("IsoCode");
            prop?.SetValue(currency, "NEW");
        };

        act.Should().Throw<InvalidOperationException>();
    }

    
}