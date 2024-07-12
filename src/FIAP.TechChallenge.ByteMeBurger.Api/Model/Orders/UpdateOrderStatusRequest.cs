// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using Microsoft.Build.Framework;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Model.Orders;

/// <summary>
/// Request to update an order status.
/// </summary>
public class  UpdateOrderStatusRequest
{
    /// <summary>
    /// Order status
    /// </summary>
    [Required]
    public OrderStatusViewModel Status { get; set; }
}
