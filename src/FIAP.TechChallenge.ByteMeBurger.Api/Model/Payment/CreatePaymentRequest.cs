using Microsoft.Build.Framework;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Model.Payment;

/// <summary>
/// Create payment request.
/// </summary>
public class CreatePaymentRequest
{
    /// <summary>
    /// Order id.
    /// </summary>
    [Required]
    public Guid OrderId { get; set; }

    /// <summary>
    /// Payment type
    /// </summary>
    [Required]
    public PaymentType PaymentType { get; set; }
}

public enum PaymentType
{
    Test = 0,
    MercadoPago = 1
}
