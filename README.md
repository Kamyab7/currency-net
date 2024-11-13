## Currency.NET
**CurrencyDotNet** is a robust .NET library for handling international currencies.
It contains all the information about the international currencies based on
[ISO 4217](https://en.wikipedia.org/wiki/ISO_4217).
You can access all of these currencies in the [`CurrencySource`]()
which is a static class. You can use it to find a specific currency by it's `curencyCode` or symbol.

### Features

- **Coverage**: Includes all standard ISO-4217 Currencies in a static class
- **Easy Retrieval**: Retrieve currencies by ISO code, numeric code, or get a complete list of available currencies.
- **Immutable Objects**: Ensures currency instances are immutable, promoting thread safety and consistency.

### How to use
You can install it via Nuget:
```bash
dotnet add package CurrencyDotNet
```
Or via the NuGet Package Manager:
```bash
Install-Package CurrencyDotNet
```
#### Creating a Custom Currency
Although CurrencyDotNet provides predefined currencies, you can also create custom currency instances if needed.
```csharp
// Creating a custom currency
Currency customCurrency = Currency.Create(
    isoCode: "XYZ",
    numericCode: "999",
    englishName: "Xyzian Dollar",
    symbol: "X$",
    decimalCount: 2,
    persianName: "دلار ایکس",
    locations: new[] { "Xyzland" },
    wikipediaUrl: "https://en.wikipedia.org/wiki/Xyzian_dollar",
    alternativeSymbols: new[] { "XYZ$", "X-Dollar" }
);
```
#### Retrieving Currencies
CurrencyDotNet provides static methods to retrieve currencies by their ISO code or numeric code, as well as to fetch all available currencies.

```csharp
// Retrieve a currency by ISO code
Currency? usd = Currency.WithCode("USD");
if (usd != null)
{
    Console.WriteLine($"USD Symbol: {usd.Symbol}");
}

// Retrieve a currency by numeric code
Currency? eur = Currency.WithNumericCode("978");
if (eur != null)
{
    Console.WriteLine($"EUR Name: {eur.Name}");
}

// Get all available currencies
var allCurrencies = Currency.GetAll();
foreach (var currency in allCurrencies.Take(5)) // Display first 5 for brevity
{
    Console.WriteLine($"{currency.IsoCode} - {currency.Name} ({currency.Symbol})");
}
```

Made with ❤️ by **imun**
