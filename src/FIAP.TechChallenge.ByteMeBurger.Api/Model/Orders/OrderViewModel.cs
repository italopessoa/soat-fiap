// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.Api.Model.Customers;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Model.Orders;

public class OrderViewModel
{
    public Guid Id { get; set; }

    public CustomerViewModel? Customer { get; set; }

    public string? TrackingCode { get; set; }

    public List<OrderItemViewModel> OrderItems { get; set; }

    public decimal Total { get; set; }

    public OrderStatusViewModel Status { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime? LastUpdate { get; set; }
}
