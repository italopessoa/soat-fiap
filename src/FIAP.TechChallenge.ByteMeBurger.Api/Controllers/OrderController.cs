using System.Collections.ObjectModel;
using FIAP.TechChallenge.ByteMeBurger.Api.Model;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Ingoing;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> Create(
            CreateOrderCommandDto newOrder,
            CancellationToken cancellationToken)
        {
            var orderItems = newOrder.Items.Select(i => (i.ProductId, i.ProductName, i.Quantity, i.UnitPrice));
            var order = await _orderService.CreateAsync(newOrder.CustomerId, orderItems.ToList());
            
            return Created($"{order.Id}", new OrderDto(order));
        }

        [HttpGet]
        public async Task<ActionResult<ReadOnlyCollection<OrderDto>>> Get(CancellationToken cancellationToken)
        {
            var orders = await _orderService.GetAllAsync();
            var ordersDto = orders.Select(o => new OrderDto(o));

            return Ok(ordersDto);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderDto>> Get(Guid id, CancellationToken cancellationToken)
        {
            if (Guid.Empty == id)
                return BadRequest("Invalid OrderId");
            
            var order = await _orderService.GetAsync(id);
            if (order is null)
                return NotFound();
            
            return Ok(new OrderDto(order));
        }
    }
}