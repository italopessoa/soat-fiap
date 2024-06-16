// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using System.Diagnostics.CodeAnalysis;
using FIAP.TechChallenge.ByteMeBurger.Application;
using FIAP.TechChallenge.ByteMeBurger.MercadoPago.Gateway;
using FIAP.TechChallenge.ByteMeBurger.MercadoPago.Gateway.Configuration;
using FIAP.TechChallenge.ByteMeBurger.Persistence;

namespace FIAP.TechChallenge.ByteMeBurger.Api;

[ExcludeFromCodeCoverage]
internal static class ServiceCollectionExtensions
{
    public static void ConfigureApiDependencyInversion(this WebApplicationBuilder builder)
    {
        ConfigureDependencies(builder.Services, builder.Configuration);
    }

    private static void ConfigureDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigurePersistenceApp(configuration);
        services.ConfigureApplicationApp();
        services.ConfigureMercadoPagoGatewayApp(configuration);
        services.AddOptionsWithValidateOnStart<MercadoPagoOptions>()
            .Bind(configuration.GetSection(MercadoPagoOptions.MercadoPago))
            .ValidateDataAnnotations();
    }
}
