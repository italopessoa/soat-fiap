using Microsoft.Build.Framework;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Model.Orders;

/// <summary>
/// Add Product to order
/// </summary>
public class CreateOrderItemDto
{
    /// <summary>
    /// Product Id
    /// </summary>
    [Required]
    public Guid ProductId { get; set; }

    /// <summary>
    /// Quantity
    /// </summary>
    [Required]
    public int Quantity { get; set; }
}
