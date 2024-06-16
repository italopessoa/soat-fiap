// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using System.Diagnostics.CodeAnalysis;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.MercadoPago.Gateway.Configuration;
using FIAP.TechChallenge.ByteMeBurger.MercadoPago.Gateway.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FIAP.TechChallenge.ByteMeBurger.MercadoPago.Gateway;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionsExtensions
{
    public static void ConfigureMercadoPagoGatewayApp(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MercadoPagoOptions>(_ => configuration.GetSection(MercadoPagoOptions.MercadoPago));

        // services.AddOptionsWithValidateOnStart<MercadoPagoOptions>()
        //     .Configure(configuration.GetSection(MercadoPagoOptions.MercadoPago))
        //     .ValidateDataAnnotations();

        services.AddSingleton<IMercadoPagoHmacSignatureValidator, MercadoPagoHmacSignatureValidator>();
        services.AddSingleton<IPaymentGateway, MercadoPagoService>();
    }
}
