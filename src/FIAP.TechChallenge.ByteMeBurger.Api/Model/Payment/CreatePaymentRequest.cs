using FIAP.TechChallenge.ByteMeBurger.Controllers.Dto;
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
    public PaymentTypeDto PaymentType { get; set; }
}
