using System.ComponentModel.DataAnnotations;
using FIAP.TechChallenge.ByteMeBurger.Api.Model.Customers;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Controllers;

/// <summary>
/// Customers controller
/// </summary>
/// <param name="customerService">Customer service (port implementation).</param>
/// <param name="logger">Logger</param>
[Route("api/[controller]")]
[Produces("application/json")]
[Consumes("application/json")]
[ApiController]
[ApiConventionType(typeof(DefaultApiConventions))]
public class CustomersController(ICustomerService customerService, ILogger<CustomersController> logger)
    : ControllerBase
{
    /// <summary>
    /// Find customers by Cpf
    /// </summary>
    /// <param name="cpf">Customer's cpf</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Customer</returns>
    [HttpGet]
    public async Task<ActionResult<CustomerDto>> Get([FromQuery] [MaxLength(14)] string cpf,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting customer by CPF: {Cpf}", cpf);
        var customer = await customerService.FindByCpfAsync(cpf);
        if (customer is null)
        {
            logger.LogWarning("Customer with CPF: {Cpf} not found", cpf);
            return NotFound();
        }

        logger.LogInformation("Customer with CPF: {Cpf} found {@customer}", cpf, customer);
        return Ok(customer.ToCustomerViewModel());
    }

    /// <summary>
    /// Create new customer
    /// </summary>
    /// <param name="createCustomerRequest">Create new customer parameters</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Customer</returns>
    [HttpPost]
    public async Task<ActionResult<CustomerDto>> Post([FromBody] CreateCustomerRequest createCustomerRequest,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating customer with CPF: {Cpf}", createCustomerRequest.Cpf);
        var customer = await customerService.CreateAsync(
            createCustomerRequest.Cpf,
            createCustomerRequest.Name,
            createCustomerRequest.Email);

        logger.LogInformation("Customer with CPF: {Cpf} created", createCustomerRequest.Cpf);
        return Ok(customer.ToCustomerViewModel());
    }
}
