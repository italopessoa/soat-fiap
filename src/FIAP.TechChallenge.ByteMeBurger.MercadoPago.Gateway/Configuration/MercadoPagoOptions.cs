using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FIAP.TechChallenge.ByteMeBurger.MercadoPago.Gateway.Configuration;

[ExcludeFromCodeCoverage]
public class MercadoPagoOptions
{
    public const string MercadoPago = "MercadoPago";

    [Required]
    [MinLength(1)]
    public string WebhookSecret { get; set; } = null!;

    [Required]
    [MinLength(1)]
    public string AccessToken { get; set; } = null!;

    [MinLength(1)]
    public string? NotificationUrl { get; set; }
}
