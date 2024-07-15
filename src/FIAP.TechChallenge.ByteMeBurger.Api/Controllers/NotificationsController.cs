// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.Api.Auth;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
using FIAP.TechChallenge.ByteMeBurger.MercadoPago.Gateway.Model;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Controllers;

/// <summary>
/// MercadoPago webhook controller
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Consumes("application/json")]
public class NotificationsController : ControllerBase
{
    private readonly IPaymentService _paymentService;
    private readonly ILogger<NotificationsController> _logger;

    public NotificationsController(IPaymentService paymentService, ILogger<NotificationsController> logger)
    {
        _paymentService = paymentService;
        _logger = logger;
    }

    /// <summary>
    /// Mercado Pago Integration endpoint
    /// </summary>
    /// <param name="event"></param>
    /// <returns></returns>
    [TypeFilter(typeof(MercadoPagoMessageAuthorizationFilter))]
    [HttpPost("mercadopago")]
    public async Task<IActionResult> Post([FromBody] MercadoPagoWebhookEvent @event)
    {
        _logger.LogInformation("Received MercadoPago webhook event {@Payload}", @event);
        if (@event.Action == "payment.updated" && await CheckIfPaymentExists(@event.Data.Id))
        {
            Response.OnCompleted(async () =>
            {
                using (_logger.BeginScope("Processing payment {PaymentId}", @event.Data.Id))
                {
                    await _paymentService.SyncPaymentStatusWithGatewayAsync(@event.Data.Id, PaymentType.MercadoPago);
                }
            });
        }

        return Ok();
    }

    private async Task<bool> CheckIfPaymentExists(string paymentId)
    {
        var paymentExists = await _paymentService.GetPaymentAsync(paymentId) is not null;
        if (!paymentExists)
        {
            _logger.LogWarning("Payment not found {PaymentId}. Message skipped.", paymentId);
        }

        return paymentExists;
    }

    /// <summary>
    /// Fake payment Integration endpoint
    /// </summary>
    [HttpPost("fakepayment")]
    public async Task<IActionResult> Post([FromBody] Guid paymentId)
    {
        _logger.LogInformation("Received FakePayment webhook event {PaymentId}", paymentId);

        if (await CheckIfPaymentExists(paymentId.ToString()))
        {
            Response.OnCompleted(async () =>
            {
                await _paymentService.SyncPaymentStatusWithGatewayAsync(paymentId.ToString(), PaymentType.Test);
            });
        }

        return Ok();
    }
}
