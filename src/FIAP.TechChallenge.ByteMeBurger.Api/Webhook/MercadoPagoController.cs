// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.MercadoPago.Gateway.Auth;
using FIAP.TechChallenge.ByteMeBurger.MercadoPago.Gateway.Model;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Webhook;

/// <summary>
/// MercadoPago webhook controller
/// </summary>
[Route("api/[controller]")]
[ApiController]
[TypeFilter(typeof(MercadoPagoMessageAuthorizationFilter))]
[Consumes("application/json")]
public class MercadoPagoController : ControllerBase
{
    /// <summary>
    /// Mercado Pago Webhook endpoint
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    [HttpPost("webhook")]
    public async Task<IActionResult> Post([FromBody] MercadoPagoWebhookEvent payload)
    {
        if (payload.Action == "payment.updated")
        {
            return Accepted();
        }

        return Ok();
    }
}
