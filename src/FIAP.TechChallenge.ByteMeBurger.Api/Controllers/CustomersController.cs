using System.ComponentModel.DataAnnotations;
using FIAP.TechChallenge.ByteMeBurger.Api.Model;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Ingoing;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController(ICustomerService customerService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<CustomerDto>> GetByCpf([FromQuery] [MaxLength(14)] string cpf,
            CancellationToken cancellationToken)
        {
            var customer = await customerService.FindByCpfAsync(cpf);
            if (customer is null)
            {
                return NotFound();
            }

            return Ok(new CustomerDto(customer));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> Create([FromBody] CreateCustomerCommand createCustomerCommand,
            CancellationToken cancellationToken)
        {
            var customer = await customerService.CreateAsync(
                createCustomerCommand.Cpf,
                createCustomerCommand.Name,
                createCustomerCommand.Email);

            return Ok(new CustomerDto(customer));
        }
    }
}