// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;

public interface IPaymentService
{
    Task<Payment> CreateOrderPaymentAsync(CreateOrderPaymentRequestDto command);

    Task<Payment?> GetPaymentAsync(string paymentId);

    Task<bool> SyncPaymentStatusWithGatewayAsync(string paymentId, PaymentType paymentType);
}

public record CreateOrderPaymentRequestDto(Guid OrderId, PaymentType PaymentType);
