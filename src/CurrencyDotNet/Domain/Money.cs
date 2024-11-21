using System.Collections.Generic;
using System.Diagnostics.Contracts;

// ReSharper disable CheckNamespace

namespace CurrencyDotNet;

/// <summary>
/// Money type with support for a standard <see cref="Currency"/>
/// </summary>
public struct Money
{
    #region  props

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
    
    #endregion
    
#pragma warning disable CS8618, CS9264
    public Money(decimal amount, string currencyCode)
#pragma warning restore CS8618, CS9264
    {
        setFields(amount, currencyCode);
    }

    #region methods

    /// <summary>
    /// Returns the <see cref="Currency"/> based on <see cref="CurrencyCode"/>
    /// </summary>
    /// <returns><see cref="Currency"/>> with the specified <see cref="CurrencyCode"/></returns>
    [Pure]
    public Currency GetCurrency()
    {
        return CurrencySource.FindByCode(CurrencyCode)!;
    }

    /// <summary>
    /// Determines if <paramref name="money"/> has the same currency as the current object.
    /// </summary>
    /// <param name="money">Money to check against</param>
    /// <returns>true if has the same Currency, otherwise false.</returns>
    [Pure]
    public bool HasSameCurrencyAs(Money money)
    {
        return money.CurrencyCode.Equals(CurrencyCode);
    }
    

    #endregion
    
    
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