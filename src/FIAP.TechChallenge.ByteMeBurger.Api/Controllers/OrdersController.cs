using System.Collections.ObjectModel;
using FIAP.TechChallenge.ByteMeBurger.Api.Model;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Ingoing;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(IOrderService orderService) : ControllerBase
    {
        [Route("checkout")]
        [HttpPost]
        public async Task<ActionResult<OrderDto>> Create(
            CreateOrderCommandDto newOrder,
            CancellationToken cancellationToken)
        {
            var orderItems = newOrder.Items.Select(i => (i.ProductId, i.ProductName, i.Quantity, i.UnitPrice));
            var order = await orderService.CreateAsync(newOrder.Cpf, orderItems.ToList());

            return Created($"{order.Id}", new OrderDto(order));
        }

        [HttpGet]
        public async Task<ActionResult<ReadOnlyCollection<OrderDto>>> Get(CancellationToken cancellationToken)
        {
            var orders = await orderService.GetAllAsync();
            var ordersDto = orders.Select(o => new OrderDto(o));

            return Ok(ordersDto);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderDto>> Get(Guid id, CancellationToken cancellationToken)
        {
            if (Guid.Empty == id)
                return BadRequest("Invalid OrderId: An order ID must not be empty.");

            var order = await orderService.GetAsync(id);
            if (order is null)
                return NotFound();

            return Ok(new OrderDto(order));
        }
    }
}