﻿using Bmb.Domain.Core.Entities;
using Bmb.Domain.Core.ValueObjects;
using Bmb.Orders.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Controllers.Dto;

namespace FIAP.TechChallenge.ByteMeBurger.Controllers;

public static class Presenter
{
    public static CustomerDto FromEntityToDto(this Customer customer)
    {
        if (customer is null) return null;

        return new CustomerDto
        {
            Id = customer.Id,
            Cpf = customer.Cpf,
            Name = customer.Name,
            Email = customer.Email
        };
    }

    public static ProductDto FromEntityToDto(this Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Category = product.Category.FromEntityToDto(),
            Price = product.Price,
            Images = product.Images.ToArray()
        };
    }

    public static IReadOnlyCollection<ProductDto> FromEntityToDto(this IEnumerable<Product> products)
    {
        return products.Select(p => p.FromEntityToDto()).ToList();
    }

    public static ProductCategoryDto FromEntityToDto(this ProductCategory category)
    {
        return (ProductCategoryDto)category;
    }

    public static OrderListItemDto FromEntityToListDto(this Order order)
    {
        return new OrderListItemDto(order.Id, order.TrackingCode.Value, order.Total,
            (OrderStatusDto)order.Status,
            order.Created,
            order.Updated);
    }

    public static IReadOnlyCollection<OrderListItemDto> FromDomainToDto(this IEnumerable<Order> orders)
    {
        return orders.Select(o => o.FromEntityToListDto()).ToList();
    }

    public static OrderDetailDto? FromEntityToDto(this Order? order)
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
            OrderItems = order.OrderItems.Select(o => o.FromEntityToDto()).ToList(),
            Customer = order.Customer is null ? null : order.Customer!.FromEntityToDto()
        };
    }

    public static OrderItemDto FromEntityToDto(this OrderItem orderItem)
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

    public static NewOrderDto FromEntityToCreatedDto(this Order order)
    {
        return new NewOrderDto(order.Id, order.TrackingCode.Value);
    }
}
