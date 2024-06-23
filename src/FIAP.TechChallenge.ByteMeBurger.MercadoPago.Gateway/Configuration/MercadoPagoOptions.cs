// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FIAP.TechChallenge.ByteMeBurger.MercadoPago.Gateway.Configuration;

[ExcludeFromCodeCoverage]
public class MercadoPagoOptions
{
    public const string MercadoPago = "MercadoPago";

    [Required]
    [MinLength(1)]
    public string WebhookSecret { get; set; }

    [Required]
    [MinLength(1)]
    public string AccessToken { get; set; }

    [MinLength(1)]
    public string? NotificationUrl { get; set; }
}
