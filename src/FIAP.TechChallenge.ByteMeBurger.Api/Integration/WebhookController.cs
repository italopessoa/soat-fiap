// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.Api.Auth;
using FIAP.TechChallenge.ByteMeBurger.MercadoPago.Gateway.Model;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Webhook;

/// <summary>
/// MercadoPago webhook controller
/// </summary>
[ApiController]
[Consumes("application/json")]
public class WebhookController : ControllerBase
{
    /// <summary>
    /// Mercado Pago Integration endpoint
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    [TypeFilter(typeof(MercadoPagoMessageAuthorizationFilter))]
    [HttpPost("mercadopago")]
    public async Task<IActionResult> Post([FromBody] MercadoPagoWebhookEvent payload)
    {
        if (payload.Action == "payment.updated")
        {
            return Accepted();
        }

        return Ok();
    }
}
