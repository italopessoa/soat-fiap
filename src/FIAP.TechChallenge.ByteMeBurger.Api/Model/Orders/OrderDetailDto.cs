using FIAP.TechChallenge.ByteMeBurger.Api.Model.Customers;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Model.Orders;

public class OrderDetailDto
{
    public Guid Id { get; set; }

    public CustomerDto? Customer { get; set; }

    public string? TrackingCode { get; set; }

    public List<OrderItemDto> OrderItems { get; set; }

    public decimal Total { get; set; }

    public OrderStatusDto Status { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime? LastUpdate { get; set; }
}
