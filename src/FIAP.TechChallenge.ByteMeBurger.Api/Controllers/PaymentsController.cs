using FIAP.TechChallenge.ByteMeBurger.Api.Model.Payment;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
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
    private readonly ILogger<PaymentsController> _logger;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="paymentService">PaymentService controller. </param>
    /// <param name="logger"></param>
    public PaymentsController(IPaymentService paymentService, ILogger<PaymentsController> logger)
    {
        _paymentService = paymentService;
        _logger = logger;
    }

    /// <summary>
    /// Create a payment for an order
    /// </summary>
    /// <param name="createPaymentRequest"></param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Payment details</returns>
    [HttpPost]
    public async Task<ActionResult<PaymentDto>> Create(
        CreatePaymentRequest createPaymentRequest, CancellationToken cancellationToken)
    {
        using (_logger.BeginScope("Creating payment for order {OrderId}", createPaymentRequest.OrderId))
        {
            var payment =
                await _paymentService.CreateOrderPaymentAsync(createPaymentRequest.ToDomain());
            return Created("", new PaymentDto(payment.Id.Value, payment.QrCode));
        }
    }

    /// <summary>
    /// Get payment status
    /// </summary>
    /// <param name="id">Payment Id.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Payment status</returns>
    [HttpGet("{id:guid}/status")]
    public async Task<ActionResult<PaymentStatusDto>> GetStatus(
        Guid id, CancellationToken cancellationToken)
    {
        var payment = await _paymentService.GetPaymentAsync(new PaymentId(id));

        if (payment is null)
            return NotFound();

        return Ok((PaymentStatusDto)(int)payment.Status);
    }
}
