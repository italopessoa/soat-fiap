namespace FIAP.TechChallenge.ByteMeBurger.Api.Model;

public class CreateOrderCommandDto
{
    public string? Cpf { get; set; } = null;
    public List<CreateOrderItemDto> Items { get; set; } = new();
}