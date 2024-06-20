// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Payment;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;

namespace FIAP.TechChallenge.ByteMeBurger.Application.Services;

public class PaymentService : IPaymentService
{
    private readonly ICreatePaymentUseCase _createOrderPaymentUseCase;
    private readonly IUpdatePaymentStatusUseCase _updatePaymentStatusUseCase;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IPaymentGateway _paymentGateway;

    public PaymentService(ICreatePaymentUseCase createOrderPaymentUseCase,
        IUpdatePaymentStatusUseCase updatePaymentStatusUseCase,
        IPaymentRepository paymentRepository,
        IPaymentGateway paymentGateway)
    {
        _createOrderPaymentUseCase = createOrderPaymentUseCase;
        _updatePaymentStatusUseCase = updatePaymentStatusUseCase;
        _paymentRepository = paymentRepository;
        _paymentGateway = paymentGateway;
    }

    public async Task<Payment> CreateOrderPaymentAsync(Guid orderId)
    {
        var payment = await _createOrderPaymentUseCase.Execute(orderId);

        await _paymentRepository.SaveAsync(payment);
        return payment;
    }

    public async Task<Payment?> GetPaymentAsync(string paymentId)
    {
        return await _paymentRepository.GetPaymentAsync(paymentId);
    }

    public async Task<bool> SyncPaymentStatusWithGatewayAsync(string paymentId)
    {
        var payment = await GetPaymentAsync(paymentId);
        if (payment is null)
            return false;

        var paymentStatus = await _paymentGateway.GetPaymentStatusAsync(paymentId);
        if (paymentStatus is null)
            return false;

        await _updatePaymentStatusUseCase.Execute(payment, paymentStatus.Value);
        return true;
    }
}
