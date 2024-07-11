// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

namespace FIAP.TechChallenge.ByteMeBurger.Api.Model.Orders;

/// <summary>
/// Orders statuses
/// </summary>
public enum OrderStatusViewModel
{
    PaymentPending = 0,
    Received = 1,
    InPreparation = 2,
    Ready = 3,
    Completed = 4
}
