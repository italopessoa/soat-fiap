// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.Domain.Base;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;

public class UpdateOrderPaymentUseCase(IOrderRepository repository) : IUpdateOrderPaymentUseCase
{
    public async Task<bool> Execute(Guid orderId, PaymentId paymentId)
    {
        var order = await repository.GetAsync(orderId);
        if (order == null)
        {
            throw new EntityNotFoundException($"Order '{orderId}' not found.");
        }

        order.SetPayment(paymentId);

        var updated = await repository.UpdateOrderPaymentAsync(order);
        return updated;
    }
}
