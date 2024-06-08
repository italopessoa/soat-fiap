// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using System.ComponentModel.DataAnnotations;
using FIAP.TechChallenge.ByteMeBurger.Api.Model;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Controllers
{
    /// <summary>
    /// Customers controller
    /// </summary>
    /// <param name="customerService">Customer service (port implementation).</param>
    /// <param name="logger">Logger</param>
    [Route("api/[controller]")]
    [ApiController]
    [ApiConventionType(typeof(DefaultApiConventions))]
    [Produces("application/json")]
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
        public async Task<ActionResult<CustomerDto>> GetByCpf([FromQuery] [MaxLength(14)] string cpf,
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
            return Ok(new CustomerDto(customer));
        }

        /// <summary>
        /// Create new customer
        /// </summary>
        /// <param name="createCustomerCommand">Create new customer parameters</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Customer</returns>
        [HttpPost]
        public async Task<ActionResult<CustomerDto>> Create([FromBody] CreateCustomerCommand createCustomerCommand,
            CancellationToken cancellationToken)
        {
            logger.LogInformation("Creating customer with CPF: {Cpf}", createCustomerCommand.Cpf);
            var customer = await customerService.CreateAsync(
                createCustomerCommand.Cpf,
                createCustomerCommand.Name,
                createCustomerCommand.Email);

            logger.LogInformation("Customer with CPF: {Cpf} created", createCustomerCommand.Cpf);
            return Ok(new CustomerDto(customer));
        }
    }
}
