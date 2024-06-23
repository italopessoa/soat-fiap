// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.Domain.Base;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Payment;

public class CreatePaymentUseCase : ICreatePaymentUseCase
{
    private readonly IPaymentGatewayFactoryMethod _paymentGatewayFactory;
    private readonly IOrderRepository _orderRepository;

    public CreatePaymentUseCase(IPaymentGatewayFactoryMethod paymentGatewayFactory, IOrderRepository orderRepository)
    {
        _paymentGatewayFactory = paymentGatewayFactory;
        _orderRepository = orderRepository;
    }

    public async Task<Domain.Entities.Payment?> Execute(Guid orderId)
    {
        var order = await _orderRepository.GetAsync(orderId);

        if (order is null)
            throw new EntityNotFoundException("Order not found.");

        if (order.PaymentId is not null)
            throw new DomainException("There's already a Payment for the order.");

        var paymentGateway = _paymentGatewayFactory.Create(PaymentType.Test);
        return await paymentGateway.CreatePaymentAsync(order);
    }
}
