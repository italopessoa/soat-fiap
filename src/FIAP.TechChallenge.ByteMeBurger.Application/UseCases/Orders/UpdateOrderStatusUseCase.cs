// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.Domain.Base;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;

public class UpdateOrderStatusUseCase(IOrderRepository repository) : IUpdateOrderStatusUseCase
{
    public async Task<Order> Execute(Guid orderId, OrderStatus newStatus)
    {
        var order = await repository.GetAsync(orderId);
        if (order == null)
        {
            throw new EntityNotFoundException($"Order '{orderId}' not found.");
        }

        if (order.Status == OrderStatus.Completed)
        {
            throw new DomainException("Order is already completed.");
        }

        Action updateStatus = newStatus switch
        {
            OrderStatus.PaymentConfirmed => order.ConfirmPayment,
            OrderStatus.Received => order.ConfirmReceiving,
            OrderStatus.InPreparation => order.InitiatePrepare,
            OrderStatus.Ready => order.FinishPreparing,
            OrderStatus.Completed => order.DeliverOrder,
            _ => throw new DomainException(nameof(newStatus), new Exception($"Invalid Status '{newStatus}'"))
        };

        updateStatus.Invoke();
        return order;
    }
}
