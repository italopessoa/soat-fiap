// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FIAP.TechChallenge.ByteMeBurger.MercadoPago.Gateway;

public static class ServiceCollectionsExtensions
{
    public static void ConfigureMercadoPagoGatewayApp(this IServiceCollection services, IConfiguration configuration)
    {
    }
}
