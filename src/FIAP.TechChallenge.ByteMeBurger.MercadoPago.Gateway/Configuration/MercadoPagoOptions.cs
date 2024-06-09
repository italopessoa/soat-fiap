// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

namespace FIAP.TechChallenge.ByteMeBurger.MercadoPago.Gateway.Configuration;

public class MercadoPagoOptions
{
    public string WebhookSecret { get; set; }
    public string AccessToken { get; set; }
}
