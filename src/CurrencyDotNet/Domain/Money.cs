using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

// ReSharper disable CheckNamespace

namespace CurrencyDotNet;

/// <summary>
/// Represents a monetary value with a specific currency.
/// </summary>
public struct Money : IEquatable<Money>
{
    #region Properties

    /// <summary>
    /// The amount of the Money.
    /// </summary>
    public decimal Amount { get; private set; }

    /// <summary>
    /// The Currency ISO Code.
    /// </summary>
    public string CurrencyCode { get; private set; }

    /// <summary>
    /// Currency Symbol like $ for USD.
    /// </summary>
    public string CurrencySymbol { get; private set; }

    /// <summary>
    /// Name of the Currency (e.g., US Dollar for USD).
    /// </summary>
    public string CurrencyName { get; private set; }
    
    /// <summary>
    /// The number of decimal places to use when formatting.
    /// </summary>
    public int DecimalPlaces { get; private set; }

    #endregion

    #pragma warning disable CS8618, CS9264
    /// <summary>
    /// Initializes a new instance of the <see cref="Money"/> struct.
    /// </summary>
    /// <param name="amount">The monetary amount.</param>
    /// <param name="currencyCode">The ISO currency code.</param>
    public Money(decimal amount, string currencyCode)
    #pragma warning restore CS8618, CS9264
    {
        setFields(amount, currencyCode);
    }

    #region Methods

    /// <summary>
    /// Returns the <see cref="Currency"/> based on <see cref="CurrencyCode"/>.
    /// </summary>
    /// <returns>The corresponding <see cref="Currency"/>.</returns>
    [Pure]
    public Currency GetCurrency()
    {
        return CurrencySource.FindByCode(CurrencyCode)!;
    }

    /// <summary>
    /// Determines if the specified <paramref name="money"/> has the same currency as the current instance.
    /// </summary>
    /// <param name="money">The Money instance to compare.</param>
    /// <returns><c>true</c> if the currencies match; otherwise, <c>false</c>.</returns>
    [Pure]
    public bool HasSameCurrencyAs(Money money)
    {
        return money.CurrencyCode.Equals(CurrencyCode, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Determines whether all Money instances in the collection have the same currency as the current instance.
    /// </summary>
    /// <param name="moneys">The collection of Money instances to compare.</param>
    /// <returns><c>true</c> if all instances have the same currency; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="moneys"/> is <c>null</c>.</exception>
    [Pure]
    public bool HasSameCurrencyAs(IEnumerable<Money> moneys)
    {
        if (moneys is null)
        {
            throw new ArgumentNullException(nameof(moneys));
        }

        return moneys.All(this.HasSameCurrencyAs);
    }

    /// <summary>
    /// Indicates whether the amount has decimal fractions.
    /// </summary>
    /// <returns><c>true</c> if the amount has decimals; otherwise, <c>false</c>.</returns>
    [Pure]
    public bool HasDecimals
    {
        get
        {
            var truncatedAmount = decimal.Truncate(Amount);
            return Amount - truncatedAmount != decimal.Zero;
        }
    }

    /// <summary>
    /// Formats the Money instance as a string.
    /// </summary>
    /// <returns>A string representation of the Money instance.</returns>
    [Pure]
    public override string ToString()
    {
        return $"{CurrencySymbol}{Amount.ToString($"N{this.DecimalPlaces}")}";
    }
    
    /// <summary>
    /// Formats the Money instance using a specified number of decimal places.
    /// </summary>
    /// <param name="decimalPlaces">The number of decimal places to format.</param>
    /// <returns>A formatted string representation of the Money instance.</returns>
    [Pure]
    public string ToString(int decimalPlaces)
    {
        return $"{CurrencySymbol}{Amount.ToString($"N{decimalPlaces}")}";
    }

    /// <summary>
    /// Formats the Money instance using a specified format.
    /// </summary>
    /// <param name="format">The format string.</param>
    /// <returns>A formatted string representation of the Money instance.</returns>
    [Pure]
    public string ToString(string format)
    {
        return $"{CurrencySymbol}{Amount.ToString(format)}";
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current Money instance.
    /// </summary>
    /// <param name="obj">The object to compare with.</param>
    /// <returns><c>true</c> if equal; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj)
    {
        return obj is Money money && Equals(money);
    }

    /// <summary>
    /// Determines whether the specified Money is equal to the current instance.
    /// </summary>
    /// <param name="other">The Money to compare with.</param>
    /// <returns><c>true</c> if equal; otherwise, <c>false</c>.</returns>
    public bool Equals(Money other)
    {
        return Amount == other.Amount && CurrencyCode.Equals(other.CurrencyCode, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Returns the hash code for the Money instance.
    /// </summary>
    /// <returns>A hash code for the current Money.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(Amount, CurrencyCode.ToUpperInvariant());
    }

    /// <summary>
    /// Adds two Money instances.
    /// </summary>
    /// <param name="a">First Money instance.</param>
    /// <param name="b">Second Money instance.</param>
    /// <returns>The sum of the two Money instances.</returns>
    /// <exception cref="InvalidOperationException">Thrown when currencies do not match.</exception>
    public static Money operator +(Money a, Money b)
    {
        if (!a.HasSameCurrencyAs(b))
            throw new InvalidOperationException("Cannot add amounts with different currencies.");

        return new Money(a.Amount + b.Amount, a.CurrencyCode);
    }

    /// <summary>
    /// Subtracts one Money instance from another.
    /// </summary>
    /// <param name="a">First Money instance.</param>
    /// <param name="b">Second Money instance.</param>
    /// <returns>The difference of the two Money instances.</returns>
    /// <exception cref="InvalidOperationException">Thrown when currencies do not match.</exception>
    public static Money operator -(Money a, Money b)
    {
        if (!a.HasSameCurrencyAs(b))
            throw new InvalidOperationException("Cannot subtract amounts with different currencies.");

        return new Money(a.Amount - b.Amount, a.CurrencyCode);
    }

    
    /// <summary>
    /// Adds the specified Money to the current instance.
    /// </summary>
    /// <param name="money">The Money instance to add.</param>
    /// <returns>A new Money instance representing the sum.</returns>
    [Pure]
    public Money Plus(Money money)
    {
        return this + money;
    }

    /// <summary>
    /// Subtracts the specified Money from the current instance.
    /// </summary>
    /// <param name="money">The Money instance to subtract.</param>
    /// <returns>A new Money instance representing the difference.</returns>
    [Pure]
    public Money Minus(Money money)
    {
        return this - money;
    }

    /// <summary>
    /// Increases the amount by the specified value.
    /// </summary>
    /// <param name="amount">The amount to increase.</param>
    public void Increase(decimal amount)
    {
        Amount += amount;
    }

    /// <summary>
    /// Decreases the amount by the specified value.
    /// </summary>
    /// <param name="amount">The amount to decrease.</param>
    public void Decrease(decimal amount)
    {
        Amount -= amount;
    }

    
    /// <summary>
    /// Calculates the total sum of a collection of Money instances.
    /// </summary>
    /// <param name="moneys">The collection of Money instances to sum.</param>
    /// <returns>The total sum as a new Money instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="moneys"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown when the collection is empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when currencies in the collection do not match.</exception>
    public static Money Total(IEnumerable<Money> moneys)
    {
        if (moneys is null)
        {
            throw new ArgumentNullException(nameof(moneys));
        }
        
        // ReSharper disable once PossibleMultipleEnumeration
        if (!moneys.Any())
        {
            throw new ArgumentException("At least one money is required.");
        }

        return moneys.Aggregate((x, y) => x + y);
    }

    /// <summary>
    /// Calculates the total sum of a parameter array of Money instances.
    /// </summary>
    /// <param name="moneys">The Money instances to sum.</param>
    /// <returns>The total sum as a new Money instance.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="moneys"/> is <c>null</c>.</exception>
    /// <exception cref="ArgumentException">Thrown when the array is empty.</exception>
    /// <exception cref="InvalidOperationException">Thrown when currencies in the array do not match.</exception>
    public static Money Total(params Money[] moneys)
    {
        return Total(moneys.AsEnumerable());
    }

    #endregion

    #region Private Methods

    private void setFields(decimal amount, string currencyCode, int? decimalPlaces = null)
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
        if (decimalPlaces is null)
        {
            DecimalPlaces = currency.DecimalPlaces;
        }
        else
        {
            DecimalPlaces = decimalPlaces.Value;
        }
    }

    #endregion
}