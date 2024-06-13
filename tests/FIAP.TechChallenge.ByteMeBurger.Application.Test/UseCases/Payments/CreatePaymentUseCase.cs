// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Payment;
using FIAP.TechChallenge.ByteMeBurger.Domain.Base;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;

namespace FIAP.TechChallenge.ByteMeBurger.Application.Test.UseCases.Payments;

public class CreatePaymentUseCase : ICreatePaymentUseCase
{
    private readonly IPaymentGateway _paymentGateway;
    private readonly IOrderRepository _orderRepository;

    public CreatePaymentUseCase(IPaymentGateway paymentGateway, IOrderRepository orderRepository)
    {
        _paymentGateway = paymentGateway;
        _orderRepository = orderRepository;
    }

    public async Task<Payment> Execute(Guid orderId)
    {
        var order = await _orderRepository.GetAsync(orderId);

        if (order is null)
            throw new EntityNotFoundException("Order not found.");

        if (order.PaymentId is not null)
            throw new DomainException("There's already a Payment for the order.");

        return await _paymentGateway.CreatePaymentAsync(order);
    }
}
