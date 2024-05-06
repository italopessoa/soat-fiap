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
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [Route("checkout")]
        [HttpPost]
        public async Task<ActionResult<OrderDto>> Create(
            CreateOrderCommandDto newOrder,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating order for customer with CPF: {Cpf}", newOrder.Cpf);
            var orderItems = newOrder.Items.Select(i => (i.ProductId, i.Quantity));
            var order = await _orderService.CreateAsync(newOrder.Cpf, orderItems.ToList());

            _logger.LogInformation("Order created with ID: {OrderId}", order.Id);
            return Created($"{order.Id}", new OrderDto(order));
        }

        [HttpGet]
        public async Task<ActionResult<ReadOnlyCollection<OrderDto>>> Get(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting all orders");
            var orders = await _orderService.GetAllAsync();
            var ordersDto = orders.Select(o => new OrderDto(o));

            _logger.LogInformation("Retrieved {Count} orders", ordersDto.Count());
            return Ok(ordersDto);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderDto>> Get(Guid id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting order with ID: {OrderId}", id);
            if (Guid.Empty == id)
                return BadRequest("Invalid OrderId: An order ID must not be empty.");

            var order = await _orderService.GetAsync(id);
            if (order is null)
            {
                _logger.LogWarning("Order with ID: {OrderId} not found", id);
                return NotFound();
            }

            _logger.LogInformation("Order with ID: {OrderId} found", id);
            return Ok(new OrderDto(order));
        }
    }
}