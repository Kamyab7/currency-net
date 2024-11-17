using System.Collections.Generic;

namespace CurrencyDotNet;

public struct Money
{
    
    /// <summary>
    /// The amount of the Money
    /// </summary>
    public decimal Amount { get; }
    
    /// <summary>
    /// The Currency ISO Code
    /// </summary>
    public string CurrencyCode { get; }
    
    /// <summary>
    /// Currency Symbol like $ for USD
    /// </summary>
    public string CurrencySymbol { get; }
    
    /// <summary>
    /// Name of the Currency (ex: Us Dollar for USD)
    /// </summary>
    public string CurrencyName { get; }

    public Money(decimal amount, string currencyCode)
    {
        Amount = amount;
        var currency = CurrencySource.FindByCode(currencyCode); 
        if (currency is null)
        {
            throw new KeyNotFoundException($"Currency with '{currencyCode}' not found");
        }
        CurrencyCode = currencyCode;
        CurrencyName = currency.Name;
        CurrencySymbol = currency.Symbol;
    }
    
}