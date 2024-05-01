namespace FIAP.TechChallenge.ByteMeBurger.Api.Model;

public class CreateOrderCommandDto
{
    public string? CustomerId { get; set; }
    public List<OrderItemDto> Items { get; set; } = new();
}