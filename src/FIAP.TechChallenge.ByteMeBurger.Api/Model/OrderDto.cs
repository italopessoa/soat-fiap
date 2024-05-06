using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Model;

public class OrderDto
{
    public OrderDto()
    {
    }

    public OrderDto(Order order)
    {
        Id = order.Id;
        TrackingCode = order.TrackingCode;
        Total = order.Total;
        Status = order.Status;
        CreationDate = order.Created;
        LastUpdate = order.LastUpdate;
        OrderItems = order.OrderItems.Select(o => new OrderItemDto(o)).ToList();
        Customer = order.Customer is null ? null : new CustomerDto(order.Customer!);
    }

    public Guid Id { get; set; }

    public CustomerDto? Customer { get; set; }

    public string? TrackingCode { get; set; }

    public List<OrderItemDto> OrderItems { get; set; }

    public decimal Total { get; set; }

    public OrderStatus Status { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime? LastUpdate { get; set; }
}