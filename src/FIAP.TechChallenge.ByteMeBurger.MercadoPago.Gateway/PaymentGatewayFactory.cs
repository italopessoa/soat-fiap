// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.Domain.Base;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
using Microsoft.Extensions.DependencyInjection;

namespace FIAP.TechChallenge.ByteMeBurger.MercadoPago.Gateway;

public sealed class PaymentGatewayFactory(IServiceProvider provider) : IPaymentGatewayFactoryMethod
{
    public IPaymentGateway Create(PaymentType paymentType)
    {
        var gateway = paymentType switch
        {
            PaymentType.MercadoPago => provider.GetKeyedService<IPaymentGateway>(
                $"Payment-{nameof(PaymentType.MercadoPago)}"),
            PaymentType.Test => provider.GetKeyedService<IPaymentGateway>($"Payment-{nameof(PaymentType.Test)}"),
            _ => null
        };

        return gateway ?? throw new DomainException($"Payment Gateway Payment-{paymentType} not found.");
    }
}
