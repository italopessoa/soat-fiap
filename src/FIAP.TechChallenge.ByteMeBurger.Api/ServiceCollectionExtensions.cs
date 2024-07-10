// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using System.Diagnostics.CodeAnalysis;
using FIAP.TechChallenge.ByteMeBurger.MercadoPago.DI;

namespace FIAP.TechChallenge.ByteMeBurger.Api;

[ExcludeFromCodeCoverage]
internal static class ServiceCollectionExtensions
{
    public static void ConfigureApiDependencyInversion(this WebApplicationBuilder builder)
    {
        builder.Services.ConfigureApplicationDI(builder.Configuration);
    }
}
