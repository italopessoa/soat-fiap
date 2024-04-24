using System.ComponentModel.DataAnnotations;
using FIAP.TechChallenge.ByteMeBurger.Api.Model;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Ingoing;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerDto>> GetByCpf([FromQuery] [MaxLength(14)] string cpf,
            CancellationToken cancellationToken)
        {
            var customer = await _customerService.FindByCpfAsync(cpf);
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
            var createCustomerTask = string.IsNullOrWhiteSpace(createCustomerCommand.Cpf)
                ? _customerService.CreateAnonymousAsync()
                : _customerService.CreateAsync(
                    createCustomerCommand.Cpf,
                    createCustomerCommand.Name ?? string.Empty,
                    createCustomerCommand.Email ?? string.Empty);
            
            await createCustomerTask.WaitAsync(cancellationToken);

            return Ok(new CustomerDto(createCustomerTask.Result));
        }
    }
}