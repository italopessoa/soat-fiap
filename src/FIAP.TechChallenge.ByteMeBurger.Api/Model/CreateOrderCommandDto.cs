namespace FIAP.TechChallenge.ByteMeBurger.Api.Model;

public class CreateOrderCommandDto
{
    public string? Cpf { get; set; } = null;
    
    // TODO remove it
    public Guid? CustomerId { get; set; } = null;
    public List<OrderItemDto> Items { get; set; } = new();
}