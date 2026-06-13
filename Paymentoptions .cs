// ============================================================
// PaymentOptions.cs
// Exercise 3: The Silent Crash — Options Pattern
//
// Strongly-typed configuration class that PREVENTS the app from
// starting when required settings are missing.
// ============================================================

using System.ComponentModel.DataAnnotations;

// TODO 1: Create a class called PaymentOptions with two validated properties.
public class PaymentOptions
{
    // [Required] causes OptionsValidationException at startup if GatewayUrl is absent.
    [Required]
    public required string GatewayUrl { get; init; }

    // [Range] enforces business rules on deposit amounts (in Ethiopian Birr).
    [Range(100, 100_000)]
    public decimal MaxDepositBirr { get; init; }
}