using FIAP.TechChallenge.ByteMeBurger.Api.Model;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Produces("application/json")]
[Consumes("application/json")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="paymentService"></param>
    public PaymentsController(IPaymentService paymentService)
    {
        _paymentService = paymentService;
    }

    /// <summary>
    /// Create a payment for an order
    /// </summary>
    /// <param name="orderId">Order id.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Payment details</returns>
    [HttpPost]
    public async Task<ActionResult<PaymentViewModel>> Create(
        [FromQuery] Guid orderId, CancellationToken cancellationToken)
    {
        var payment = await _paymentService.CreateOrderPaymentAsync(orderId);
        return Ok(new PaymentViewModel(payment.Id.Code, payment.QrCode));
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
