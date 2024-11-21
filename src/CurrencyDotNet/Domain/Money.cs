using System.Collections.Generic;
// ReSharper disable CheckNamespace

namespace CurrencyDotNet;

/// <summary>
/// Money type with support for a standard <see cref="Currency"/>
/// </summary>
public struct Money
{
    
    /// <summary>
    /// The amount of the Money
    /// </summary>
    public decimal Amount { get; private set; }
    
    /// <summary>
    /// The Currency ISO Code
    /// </summary>
    public string CurrencyCode { get; private set; }
    
    /// <summary>
    /// Currency Symbol like $ for USD
    /// </summary>
    public string CurrencySymbol { get; private set; }
    
    /// <summary>
    /// Name of the Currency (ex: Us Dollar for USD)
    /// </summary>
    public string CurrencyName { get; private set; }

#pragma warning disable CS8618, CS9264
    public Money(decimal amount, string currencyCode)
#pragma warning restore CS8618, CS9264
    {
        setFields(amount, currencyCode);
    }


    public Currency GetCurrency()
    {
        return CurrencySource.FindByCode(CurrencyCode)!;
    }
    
    private void setFields(decimal amount, string currencyCode)
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