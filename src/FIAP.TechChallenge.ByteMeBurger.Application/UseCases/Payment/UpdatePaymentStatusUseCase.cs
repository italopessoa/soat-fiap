// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;
using FIAP.TechChallenge.ByteMeBurger.Domain.Events;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Payment;

public class UpdatePaymentStatusUseCase : IUpdatePaymentStatusUseCase
{
    private readonly IUpdateOrderStatusUseCase _updateOrderStatusUseCase;
    private readonly IPaymentRepository _paymentRepository;

    public UpdatePaymentStatusUseCase(IUpdateOrderStatusUseCase updateOrderStatusUseCase,
        IPaymentRepository paymentRepository)
    {
        _updateOrderStatusUseCase = updateOrderStatusUseCase;
        _paymentRepository = paymentRepository;
    }

    public async Task<bool> Execute(Domain.Entities.Payment? payment, PaymentStatus status)
    {
        if (payment is not null && payment.Status
                is not PaymentStatus.Paid
                and not PaymentStatus.Cancelled
                and not PaymentStatus.Rejected)
        {
            payment.Status = status;
            payment.Updated = DateTime.UtcNow;

            var paymentStatusUpdated = await _paymentRepository.UpdatePaymentStatusAsync(payment);

            if (paymentStatusUpdated && status is PaymentStatus.Approved)
            {
                DomainEventTrigger.RaiseOrderPaymentConfirmed(payment.Id.OrderId);
                // TODO change to eventual consistency. use events to update order status
                await _updateOrderStatusUseCase.Execute(payment.Id.OrderId, OrderStatus.InPreparation);
            }

            return true;
        }

        return false;
    }
}
