// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

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

    internal static OrderListViewModel ToOrderListViewModel(this Order order)
    {
        return new OrderListViewModel(order.Id, order.TrackingCode.Value, order.Total,
            (OrderStatusDto)order.Status,
            order.Created,
            order.Updated);
    }

    internal static OrderViewModel ToOrderViewModel(this Order order)
    {
        return new OrderViewModel
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

    internal static OrderItemViewModel ToOrderItemViewModel(this OrderItem orderItem)
    {
        return new OrderItemViewModel()
        {
            Id = orderItem.Id,
            OrderId = orderItem.OrderId,
            ProductId = orderItem.ProductId,
            Quantity = orderItem.Quantity,
            UnitPrice = orderItem.UnitPrice,
            ProductName = orderItem.ProductName
        };
    }

    internal static CustomerViewModel ToCustomerViewModel(this Customer customer)
    {
        return new CustomerViewModel
        {
            Id = customer.Id,
            Cpf = customer.Cpf,
            Name = customer.Name,
            Email = customer.Email
        };
    }

    internal static ProductViewModel ToProductViewModel(this Product product)
    {
        return new ProductViewModel
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
