using System.Collections.ObjectModel;
using FIAP.TechChallenge.ByteMeBurger.Api.Model;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Ingoing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Produces("application/json")]
    public class OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
        : ControllerBase
    {
        [Route("checkout")]
        [HttpPost]
        public async Task<ActionResult<OrderDto>> Create(
            CreateOrderCommandDto newOrder,
            CancellationToken cancellationToken)
        {
            logger.LogInformation("Creating order for customer with CPF: {Cpf}", newOrder.Cpf);
            var orderItems = newOrder.Items.Select(i => (i.ProductId, i.Quantity));
            var order = await orderService.CreateAsync(newOrder.Cpf, orderItems.ToList());

            logger.LogInformation("Order created with ID: {OrderId}", order.Id);
            return Created($"{order.Id}", new OrderDto(order));
        }

        [HttpGet]
        public async Task<ActionResult<ReadOnlyCollection<OrderDto>>> Get(CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting all orders");
            var orders = await orderService.GetAllAsync();
            var ordersDto = orders.Select(o => new OrderDto(o));

            logger.LogInformation("Retrieved {Count} orders", ordersDto.Count());
            return Ok(ordersDto);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderDto>> Get(Guid id, CancellationToken cancellationToken)
        {
            logger.LogInformation("Getting order with ID: {OrderId}", id);
            if (Guid.Empty == id)
                return BadRequest("Invalid OrderId: An order ID must not be empty.");

            var order = await orderService.GetAsync(id);
            if (order is null)
            {
                logger.LogWarning("Order with ID: {OrderId} not found", id);
                return NotFound();
            }

            logger.LogInformation("Order with ID: {OrderId} found", id);
            return Ok(new OrderDto(order));
        }
    }
}