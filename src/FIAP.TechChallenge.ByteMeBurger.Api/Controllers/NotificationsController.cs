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
    public async Task<IActionResult> Post([FromBody] MercadoPagoWebhookEvent @event,
        [FromHeader(Name = "x-signature")] string xSignature,
        [FromHeader(Name = "x-request-id")] string xRequestId)
    {
        _logger.LogInformation("Received MercadoPago webhook event {@Payload}", @event);
        if (@event.Action == "payment.updated" && await CheckIfPaymentExists(@event.Data.Id, PaymentType.MercadoPago))
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

    private async Task<bool> CheckIfPaymentExists(string paymentId, PaymentType paymentType)
    {
        var paymentExists = await _paymentService.GetPaymentAsync(paymentId, paymentType) is not null;
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
    public async Task<IActionResult> Post([FromBody] string externalReference)
    {
        _logger.LogInformation("Received FakePayment webhook event {ExternalReference}", externalReference);

        if (await CheckIfPaymentExists(externalReference, PaymentType.Test))
        {
            Response.OnCompleted(async () =>
            {
                await _paymentService.SyncPaymentStatusWithGatewayAsync(externalReference, PaymentType.Test);
            });
        }

        return Ok();
    }
}
