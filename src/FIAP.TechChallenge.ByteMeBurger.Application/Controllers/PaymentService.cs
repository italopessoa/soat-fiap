// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Payment;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Events;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.Controllers;

public class PaymentService : IPaymentService
{
    private readonly ICreatePaymentUseCase _createOrderPaymentUseCase;
    private readonly IUpdatePaymentStatusUseCase _updatePaymentStatusUseCase;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IUpdateOrderPaymentUseCase _updateOrderPaymentUseCase;
    private readonly IPaymentGatewayFactoryMethod _paymentGatewayFactory;

    public PaymentService(ICreatePaymentUseCase createOrderPaymentUseCase,
        IUpdatePaymentStatusUseCase updatePaymentStatusUseCase,
        IPaymentRepository paymentRepository,
        IUpdateOrderPaymentUseCase updateOrderPaymentUseCase,
        IPaymentGatewayFactoryMethod paymentGatewayFactory)
    {
        _createOrderPaymentUseCase = createOrderPaymentUseCase;
        _updatePaymentStatusUseCase = updatePaymentStatusUseCase;
        _paymentRepository = paymentRepository;
        _updateOrderPaymentUseCase = updateOrderPaymentUseCase;
        _paymentGatewayFactory = paymentGatewayFactory;
    }

    public async Task<Payment> CreateOrderPaymentAsync(CreateOrderPaymentRequestDto command)
    {
        var payment = await _createOrderPaymentUseCase.Execute(command.OrderId, command.PaymentType);
        if (payment is null)
            return null;

        await _paymentRepository.SaveAsync(payment);
        DomainEventTrigger.RaisePaymentCreated(new PaymentCreated(payment));
        await _updateOrderPaymentUseCase.Execute(payment.OrderId, payment.Id);
        return payment;
    }

    public async Task<Payment?> GetPaymentAsync(PaymentId paymentId)
    {
        return await _paymentRepository.GetPaymentAsync(paymentId);
    }

    public async Task<Payment?> GetPaymentAsync(string paymentId, PaymentType paymentType)
    {
        return await _paymentRepository.GetPaymentAsync(paymentId, paymentType);
    }

    public async Task<bool> SyncPaymentStatusWithGatewayAsync(string externalReference, PaymentType paymentType)
    {
        var payment = await GetPaymentAsync(externalReference, paymentType);
        if (payment is null)
            return false;

        var paymentGateway = _paymentGatewayFactory.Create(paymentType);
        var paymentStatus = await paymentGateway.GetPaymentStatusAsync(externalReference);
        if (paymentStatus is null)
            return false;

        await _updatePaymentStatusUseCase.Execute(payment, paymentStatus.Value);
        return true;
    }
}
