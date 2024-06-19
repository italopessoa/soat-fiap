// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

namespace FIAP.TechChallenge.ByteMeBurger.Persistence.Dto;

public class PaymentDAO
{
    public string Id { get; init; }
    public Guid OrderId { get; init; }
    public int Status { get; init; }
    public int PaymentType { get; init; }
    public decimal Amount { get; init; }
    public DateTime Created { get; init; }
    public DateTime? Updated { get; init; }

    public PaymentDAO()
    {
    }

    public PaymentDAO(
        string id,
        Guid orderId,
        int status,
        int paymentType,
        decimal amount,
        DateTime created,
        DateTime? updated)
    {
        Id = id;
        OrderId = orderId;
        Status = status;
        PaymentType = paymentType;
        Amount = amount;
        Created = created;
        Updated = updated;
    }
}
