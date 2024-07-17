// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.Domain.Events;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Payment;

public class UpdatePaymentStatusUseCase : IUpdatePaymentStatusUseCase
{
    private readonly IPaymentRepository _paymentRepository;

    public UpdatePaymentStatusUseCase(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }

    public async Task<bool> Execute(Domain.Entities.Payment? payment, PaymentStatus status)
    {
        if (payment is not null && payment.Status
                is not PaymentStatus.Approved
                and not PaymentStatus.Cancelled
                and not PaymentStatus.Rejected)
        {
            payment.Status = status;
            payment.Updated = DateTime.UtcNow;

            var paymentStatusUpdated = await _paymentRepository.UpdatePaymentStatusAsync(payment);

            if (paymentStatusUpdated && payment.IsApproved())
            {
                DomainEventTrigger.RaisePaymentConfirmed(payment);
            }

            return paymentStatusUpdated;
        }

        return false;
    }
}
