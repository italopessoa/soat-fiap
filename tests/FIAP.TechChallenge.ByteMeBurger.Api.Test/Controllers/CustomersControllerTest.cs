// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.Api.Controllers;
using FIAP.TechChallenge.ByteMeBurger.Api.Model;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Ingoing;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
using FluentAssertions;
using FluentAssertions.Execution;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Test.Controllers;

[TestSubject(typeof(CustomersController))]
public class CustomersControllerTest
{
    private readonly Mock<ICustomerService> _serviceMock;
    private readonly CustomersController _target;
    private static readonly Cpf cpf = "863.917.790-23";

    public CustomersControllerTest()
    {
        _serviceMock = new Mock<ICustomerService>();
        _target = new CustomersController(_serviceMock.Object, Mock.Of<ILogger<CustomersController>>());
    }

    [Fact]
    public async Task FindByCpf_NotFound()
    {
        // Arrange
        _serviceMock.Setup(s => s.FindByCpfAsync(It.IsAny<string>()))
            .ReturnsAsync(default(Customer));

        // Act
        var response = await _target.GetByCpf("863.917.790-23", CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            response.Result.Should().BeOfType<NotFoundResult>();
            response.Value.Should().BeNull();
        }

        _serviceMock.VerifyAll();
    }

    [Fact]
    public async Task FindByCpf_Success()
    {
        // Arrange
        var customerId = Guid.NewGuid();
        var expectedCustomer = new CustomerDto
        {
            Id = customerId,
            Name = "customer name",
            Email = "customer@email.com",
            Cpf = cpf
        };
        _serviceMock.Setup(s => s.FindByCpfAsync(It.IsAny<string>()))
            .ReturnsAsync(new Customer(customerId, cpf, "customer name", "customer@email.com"));

        // Act
        var response = await _target.GetByCpf("863.917.790-23", CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            response.Result.Should().BeOfType<OkObjectResult>();
            response.Result.As<OkObjectResult>()
                .Value
                .Should()
                .BeEquivalentTo(expectedCustomer, o => o.ComparingByMembers<Customer>());
        }

        _serviceMock.VerifyAll();
    }

    [Fact]
    public async Task Create_CpfOnly_Customer()
    {
        // Arrange
        var newCustomer = new Customer(cpf);
        var expectedCustomer = new CustomerDto(newCustomer);
        var command = new CreateCustomerCommand()
        {
            Cpf = cpf
        };

        _serviceMock.Setup(s =>
                s.CreateAsync(It.Is<string>(s => s == cpf), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(newCustomer);

        // Act
        var response = await _target.Create(command, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            response.Result.Should().BeOfType<OkObjectResult>();
            response.Result.As<OkObjectResult>().Value.Should()
                .BeEquivalentTo(expectedCustomer, o => o.ComparingByMembers<CustomerDto>());
        }

        _serviceMock.VerifyAll();
    }

    [Theory]
    [InlineData("customer name", "customer@email.com")]
    public async Task Create_Full_Customer(string name, string email)
    {
        // Arrange
        var newCustomer = new Customer(cpf, name, email);
        var expectedCustomer = new CustomerDto(newCustomer);
        var command = new CreateCustomerCommand()
        {
            Cpf = cpf,
            Name = name,
            Email = email
        };

        _serviceMock.Setup(s =>
                s.CreateAsync(
                    It.Is<string>(x => x == command.Cpf),
                    It.Is<string>(x => x == command.Name),
                    It.Is<string>(x => x == command.Email)))
            .ReturnsAsync(newCustomer);

        // Act
        var response = await _target.Create(command, CancellationToken.None);

        // Assert
        using (new AssertionScope())
        {
            response.Result.Should().BeOfType<OkObjectResult>();
            response.Result.As<OkObjectResult>().Value.Should().BeEquivalentTo(expectedCustomer);
        }

        _serviceMock.VerifyAll();
    }
}
