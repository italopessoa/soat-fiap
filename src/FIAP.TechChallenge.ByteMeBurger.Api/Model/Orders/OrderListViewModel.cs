// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

namespace FIAP.TechChallenge.ByteMeBurger.Api.Model.Orders;

public record OrderListViewModel(
    Guid OrderId,
    string TrackingCode,
    decimal Total,
    OrderStatusDto Status,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
