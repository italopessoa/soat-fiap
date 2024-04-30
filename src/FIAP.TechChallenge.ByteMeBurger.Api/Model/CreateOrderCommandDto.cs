using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Model;

public class CreateOrderCommandDto
{
    public string? CustomerId { get; set; }
    public List<OrderItemDto> Items { get; set; }
}

public class OrderItemDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string ProductName { get; set; }

    public OrderItemDto()
    {
        
    }
    public OrderItemDto(OrderItem orderItem)
    {
        Id = orderItem.Id;
        ProductId = orderItem.ProductId;
        Quantity = orderItem.Quantity;
        UnitPrice = orderItem.UnitPrice;
        ProductName = orderItem.ProductName;
    }
}