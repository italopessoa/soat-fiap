// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

namespace FIAP.TechChallenge.ByteMeBurger.Api.Model;

/// <summary>
/// Represents the payment status view model.
/// </summary>
public enum PaymentStatusViewModel
{
    Pending = 0,
    InProgress = 1,
    Approved = 2,
    Rejected = 3,
    Paid = 4,
    Cancelled = 5
}
