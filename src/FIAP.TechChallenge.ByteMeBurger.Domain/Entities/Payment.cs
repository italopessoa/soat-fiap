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
    public PaymentStatus Status { get; set; }

    public string Type { get; set; }

    /// <summary>
    /// Local=0, MercadoPago=1
    /// </summary>
    public int PaymentType { get; set; }

    public string QrCode { get; set; }

    public decimal Amount { get; set; }

    public Payment()
    {
    }

    public Payment(PaymentId id)
        : base(id)
    {
        Id = id;
        Status = PaymentStatus.Pending;
    }

    public Payment(PaymentId id, string qrCode, decimal amount)
        : base(id)
    {
        Id = id;
        Status = PaymentStatus.Pending;
        QrCode = qrCode;
        Amount = amount;
    }
}
