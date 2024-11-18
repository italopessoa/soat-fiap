using Bmb.Orders.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Controllers;
using FIAP.TechChallenge.ByteMeBurger.Controllers.Dto;

namespace FIAP.TechChallenge.ByteMeBurger.Api;

internal static class Mappers
{
    internal static OrderDetailDto? ToOrderViewModel(this Order? order)
    {
        if (order is null) return null;
        return new OrderDetailDto
        {
            Id = order.Id,
            TrackingCode = order.TrackingCode.Value,
            Total = order.Total,
            Status = (OrderStatusDto)order.Status,
            CreationDate = order.Created,
            LastUpdate = order.Updated,
            OrderItems = order.OrderItems.Select(o => o.ToOrderItemViewModel()).ToList(),
            Customer = order.Customer is null ? null : order.Customer!.FromEntityToDto()
        };
    }

    internal static OrderItemDto ToOrderItemViewModel(this OrderItem orderItem)
    {
        return new OrderItemDto()
        {
            OrderId = orderItem.OrderId,
            ProductId = orderItem.ProductId,
            Quantity = orderItem.Quantity,
            UnitPrice = orderItem.UnitPrice,
            ProductName = orderItem.ProductName
        };
    }
}
