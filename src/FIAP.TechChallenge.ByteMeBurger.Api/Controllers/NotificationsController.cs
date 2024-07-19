using FIAP.TechChallenge.ByteMeBurger.Api.Auth;
using FIAP.TechChallenge.ByteMeBurger.Controllers.Contracts;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
using FIAP.TechChallenge.ByteMeBurger.MercadoPago.Gateway.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Controllers;

/// <summary>
/// MercadoPago webhook controller
/// </summary>
[Route("api/[controller]")]
[ApiController]
[Consumes("application/json")]
[AllowAnonymous]
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
    public async Task<IActionResult> Post([FromBody] MercadoPagoWebhookEvent @event,
        [FromHeader(Name = "x-signature")] string xSignature,
        [FromHeader(Name = "x-request-id")] string xRequestId)
    {
        _logger.LogInformation("Received MercadoPago webhook event {@Payload}", @event);
        if (@event.Action == "payment.updated")
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

    /// <summary>
    /// Fake payment Integration endpoint
    /// </summary>
    [HttpPost("fakepayment")]
    public async Task<IActionResult> Post([FromBody] string externalReference)
    {
        _logger.LogInformation("Received FakePayment webhook event {ExternalReference}", externalReference);

        Response.OnCompleted(async () =>
        {
            await _paymentService.SyncPaymentStatusWithGatewayAsync(externalReference, PaymentType.Test);
        });

        return Ok();
    }
}
