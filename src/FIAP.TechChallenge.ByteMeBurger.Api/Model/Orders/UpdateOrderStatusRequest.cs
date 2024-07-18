using Microsoft.Build.Framework;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Model.Orders;

/// <summary>
/// Request to update an order status.
/// </summary>
public class  UpdateOrderStatusRequest
{
    /// <summary>
    /// Order status
    /// </summary>
    [Required]
    public OrderStatusDto Status { get; set; }
}
