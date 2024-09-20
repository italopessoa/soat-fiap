namespace FIAP.TechChallenge.ByteMeBurger.Persistence.Dto;

internal class OrderItemDto
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
