using System;

// ReSharper disable once CheckNamespace
namespace CurrencyDotNet;

public class DuplicateCurrencyWithIsoCodeException(string isoCode)
    : Exception(message: $"Currency with IsoCode: '{isoCode}' already exists!");