namespace FIAP.TechChallenge.ByteMeBurger.Api.Model.Orders;

/// <summary>
/// Create new order
/// </summary>
public class CreateOrderRequest
{
    /// <summary>
    /// Customer Cpf
    /// </summary>
    public string? Cpf { get; set; } = null;

    /// <summary>
    /// Order items
    /// </summary>
    public List<CreateOrderItemDto> Items { get; set; } = new();
}
