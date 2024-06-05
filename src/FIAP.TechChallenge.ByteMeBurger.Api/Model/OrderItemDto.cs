// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Model;

public class OrderItemDto
{
    public Guid Id { get; set; }

    public Guid OrderId { get; set; }

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
        OrderId = orderItem.OrderId;
        ProductId = orderItem.ProductId;
        Quantity = orderItem.Quantity;
        UnitPrice = orderItem.UnitPrice;
        ProductName = orderItem.ProductName;
    }
}

public class CreateOrderItemDto
{
    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public CreateOrderItemDto()
    {
    }
}
