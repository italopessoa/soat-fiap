// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.Domain.Base;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

public class Payment : Entity<PaymentId>, IAggregateRoot
{
    public Guid OrderId { get; set; }

    public PaymentStatus Status { get; set; }

    public string SystemId { get; set; }

    public string QrCode { get; set; }

    public Payment()
    {
    }

    public Payment(PaymentId id, Guid orderId)
        : base(id)
    {
        Id = id;
        OrderId = orderId;
        Status = PaymentStatus.Pending;
    }

    public Payment(PaymentId id, Guid orderId, string qrCode)
        : base(id)
    {
        Id = id;
        OrderId = orderId;
        Status = PaymentStatus.Pending;
        QrCode = qrCode;
    }
}
