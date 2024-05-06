using System.ComponentModel.DataAnnotations;
using FIAP.TechChallenge.ByteMeBurger.Api.Model;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Ingoing;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Produces("application/json")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(ICustomerService customerService, ILogger<CustomersController> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerDto>> GetByCpf([FromQuery] [MaxLength(14)] string cpf,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Getting customer by CPF: {Cpf}", cpf);
            var customer = await _customerService.FindByCpfAsync(cpf);
            if (customer is null)
            {
                _logger.LogWarning("Customer with CPF: {Cpf} not found", cpf);
                return NotFound();
            }

            _logger.LogInformation("Customer with CPF: {Cpf} found @{customer}", cpf, customer);
            return Ok(new CustomerDto(customer));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> Create([FromBody] CreateCustomerCommand createCustomerCommand,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating customer with CPF: {Cpf}", createCustomerCommand.Cpf);
            var customer = await _customerService.CreateAsync(
                createCustomerCommand.Cpf,
                createCustomerCommand.Name,
                createCustomerCommand.Email);

            _logger.LogInformation("Customer with CPF: {Cpf} created", createCustomerCommand.Cpf);
            return Ok(new CustomerDto(customer));
        }
    }
}