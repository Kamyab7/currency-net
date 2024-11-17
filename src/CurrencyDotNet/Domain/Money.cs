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

    public Money(decimal amount, string currencyCode)
    {
        Amount = amount;
        if (CurrencySource.FindByCode(currencyCode) is null)
        {
            throw new KeyNotFoundException($"Currency with '{currencyCode}' not found");
        }
        CurrencyCode = currencyCode;
    }
}