using System.Collections.ObjectModel;
using FIAP.TechChallenge.ByteMeBurger.Api.Auth;
using FIAP.TechChallenge.ByteMeBurger.Api.Model.Orders;
using FIAP.TechChallenge.ByteMeBurger.Controllers.Contracts;
using FIAP.TechChallenge.ByteMeBurger.Controllers.Dto;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Hybrid;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Controllers;

/// <summary>
/// Order controller
/// </summary>
/// <param name="orderService">Orders service</param>
/// <param name="logger">Logger</param>
/// <param name="cache">Cache object</param>
[Route("api/[controller]")]
[ApiController]
[ApiConventionType(typeof(DefaultApiConventions))]
[Produces("application/json")]
[Consumes("application/json")]
[Authorize]
public class OrdersController(IOrderService orderService, ILogger<OrdersController> logger, HybridCache cache)
    : ControllerBase
{
    /// <summary>
    /// Create a new order with selected items (Checkout)
    /// </summary>
    /// <param name="newOrder">Create new order command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Order</returns>
    [HttpPost]
    public async Task<ActionResult<NewOrderDto>> Post(
        CreateOrderRequest newOrder,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating order for customer with CPF: {Cpf}", newOrder.Cpf);
        var orderItems = newOrder.Items.Select(i => new SelectedProduct(i.ProductId, i.Quantity));
        var order = await orderService.CreateAsync(HttpContext.GetCustomerFromClaims(), orderItems.ToList());

        logger.LogInformation("Order created with ID: {OrderId}", order.Id);
        return CreatedAtAction(nameof(Get), order);
    }

    /// <summary>
    /// Get all orders that are not Completed
    /// </summary>
    /// <param name="listAll">If true it will return all orders. If false it returns only orders
    /// with status (Received, In Preparation or Ready).</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Orders list</returns>
    [HttpGet]
    [Authorize(Roles = $"{BmbRoles.Admin},{BmbRoles.Kitchen}")]
    public async Task<ActionResult<ReadOnlyCollection<OrderListItemDto>>> Get(bool listAll,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all orders");

        var ordersDto = await cache.GetOrCreateAsync($"orders-{(listAll ? "nonFilter" : "filter")}",
            async cancel => await orderService.GetAllAsync(listAll), token: cancellationToken);

        logger.LogInformation("Retrieved {Count} orders", ordersDto.Count);
        return Ok(ordersDto);
    }

    /// <summary>
    /// Return Order details by Id
    /// </summary>
    /// <param name="id">Order id.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Order details</returns>
    [HttpGet("{id:guid}")]
    [Authorize(Roles = $"{BmbRoles.Admin},{BmbRoles.Kitchen}")]
    public async Task<ActionResult<OrderDetailDto>> Get(Guid id, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting order with ID: {OrderId}", id);
        if (Guid.Empty == id)
            return BadRequest("Invalid OrderId: An order ID must not be empty.");

        var order = await cache.GetOrCreateAsync($"order-{id}",
            async cancel => await orderService.GetAsync(id), token: cancellationToken);

        if (order is null)
        {
            logger.LogWarning("Order with ID: {OrderId} not found", id);
            return NotFound();
        }

        logger.LogInformation("Order with ID: {OrderId} found", id);
        return Ok(order);
    }

    /// <summary>
    /// Update order status
    /// </summary>
    /// <param name="id"></param>
    /// <param name="command">Checkout order command.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    [Route("{id:guid}/status")]
    [HttpPatch]
    [Authorize(Roles = $"{BmbRoles.Admin}")]
    public async Task<ActionResult<OrderDetailDto>> Patch(Guid id,
        [FromBody] UpdateOrderStatusRequest command,
        CancellationToken cancellationToken)
    {
        using (logger.BeginScope("Updating order {OrderId} status", id))
        {
            if (await orderService.UpdateStatusAsync(id, (OrderStatus)command.Status))
            {
                return NoContent();
            }

            return BadRequest("Status not updated.");
        }
    }
}
