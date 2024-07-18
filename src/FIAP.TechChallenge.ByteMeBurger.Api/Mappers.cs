using System.Collections.ObjectModel;
using FIAP.TechChallenge.ByteMeBurger.Api.Model.Customers;
using FIAP.TechChallenge.ByteMeBurger.Api.Model.Orders;
using FIAP.TechChallenge.ByteMeBurger.Api.Model.Payment;
using FIAP.TechChallenge.ByteMeBurger.Api.Model.Products;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using PaymentType = FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects.PaymentType;

namespace FIAP.TechChallenge.ByteMeBurger.Api;

internal static class Mappers
{
    /// <summary>
    /// Convert CreatePaymentRequest to CreateOrderPaymentRequestDto.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    internal static CreateOrderPaymentRequestDto ToDomain(this CreatePaymentRequest request)
    {
        return new CreateOrderPaymentRequestDto(request.OrderId, (PaymentType)request.PaymentType);
    }

    internal static OrderListItemDto ToOrderListViewModel(this Order order)
    {
        return new OrderListItemDto(order.Id, order.TrackingCode.Value, order.Total,
            (OrderStatusDto)order.Status,
            order.Created,
            order.Updated);
    }

    internal static IReadOnlyCollection<OrderListItemDto> ToOrderListViewModel(this ReadOnlyCollection<Order> orders)
    {
        return orders.Select(o => o.ToOrderListViewModel()).ToList();
    }

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
            Customer = order.Customer is null ? null : order.Customer!.ToCustomerViewModel()
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

    internal static CustomerDto ToCustomerViewModel(this Customer customer)
    {
        return new CustomerDto
        {
            Id = customer.Id,
            Cpf = customer.Cpf,
            Name = customer.Name,
            Email = customer.Email
        };
    }

    internal static ProductDto ToProductViewModel(this Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Category = product.Category,
            Price = product.Price,
            Images = product.Images.ToArray()
        };
    }
}
