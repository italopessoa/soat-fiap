// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

public class Payment
{
    public PaymentId Id { get; set; }

    public Guid OrderId { get; set; }

    public string Status { get; set; }

    public string SystemId { get; set; }

    public Payment()
    {
    }

    public Payment(PaymentId id, Guid orderId)
    {
        Id = id;
        OrderId = orderId;
        Status = "pending";
    }
}
