// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.Api.Model.Payment;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Controllers;

/// <summary>
/// Payment controller
/// </summary>
[Route("api/[controller]")]
[ApiController]
[ApiConventionType(typeof(DefaultApiConventions))]
[Produces("application/json")]
[Consumes("application/json")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="paymentService">PaymentService controller. </param>
    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    /// <summary>
    /// Create a payment for an order
    /// </summary>
    /// <param name="createPaymentRequest"></param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Payment details</returns>
    [HttpPost]
    public async Task<ActionResult<PaymentViewModel>> Create(
        CreatePaymentRequest createPaymentRequest, CancellationToken cancellationToken)
    {
        var payment =
            await _paymentService.CreateOrderPaymentAsync(createPaymentRequest.ToDomain());
        return Created("", new PaymentViewModel(payment.Id.Code, payment.QrCode));
    }

    /// <summary>
    /// Get payment status
    /// </summary>
    /// <param name="id">Payment Id.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Payment status</returns>
    [HttpGet("{id}/status")]
    public async Task<ActionResult<PaymentStatusViewModel>> GetStatus(
        string id, CancellationToken cancellationToken)
    {
        var payment = await _paymentService.GetPaymentAsync(id);

        if (payment is null)
            return NotFound();

        return Ok((PaymentStatusViewModel)(int)payment.Status);
    }
}
