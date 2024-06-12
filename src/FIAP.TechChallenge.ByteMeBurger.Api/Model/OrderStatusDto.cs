// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

namespace FIAP.TechChallenge.ByteMeBurger.Api.Model;

/// <summary>
/// Orders statuses
/// </summary>
public enum OrderStatusDto
{
    PaymentPending = 0,
    PaymentConfirmed = 1,
    Received = 2,
    InPreparation = 3,
    Ready = 4,
    Completed = 5
}
