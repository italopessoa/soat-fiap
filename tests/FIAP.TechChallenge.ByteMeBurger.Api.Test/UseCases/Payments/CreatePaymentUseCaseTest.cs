// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using AutoFixture;
using FIAP.TechChallenge.ByteMeBurger.Application.Test.UseCases.Payments;
using FIAP.TechChallenge.ByteMeBurger.Domain.Base;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
using FluentAssertions;
using FluentAssertions.Execution;
using JetBrains.Annotations;
using Moq;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Test.UseCases.Payments;

[TestSubject(typeof(CreatePaymentUseCase))]
public class CreatePaymentUseCaseTest
{
    private readonly Mock<IPaymentGateway> _paymentGatewayMock;
    private readonly CreatePaymentUseCase _createPaymentUseCase;
    private readonly Mock<IOrderRepository> _orderRepository;

    public CreatePaymentUseCaseTest()
    {
        _orderRepository = new Mock<IOrderRepository>();
        _paymentGatewayMock = new Mock<IPaymentGateway>();
        _createPaymentUseCase = new CreatePaymentUseCase(_paymentGatewayMock.Object, _orderRepository.Object);
    }

    [Fact]
    public async Task Execute_ValidOrder_ShouldReturnPayment()
    {
        // Arrange
        var fixture = new Fixture();
        var order = fixture.Build<Order>()
            .Without(o => o.PaymentId)
            .Create();

        var expectedPayment = fixture.Build<Payment>()
            .With(p => p.Id, new PaymentId("paymentId", 0))
            .With(p => p.Status, "pending")
            .Create();

        _orderRepository.Setup(o => o.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(order);
        _paymentGatewayMock.Setup(ps => ps.CreatePaymentAsync(It.IsAny<Order>()))
            .ReturnsAsync(expectedPayment);

        // Act
        var payment = await _createPaymentUseCase.Execute(Guid.NewGuid());

        // Assert
        using var scope = new AssertionScope();
        payment.Should().NotBeNull();
        payment.Status.Should().Be("pending");
        _paymentGatewayMock.Verify(ps => ps.CreatePaymentAsync(It.IsAny<Order>()), Times.Once);
    }

    [Fact]
    public async Task Execute_OrderNotFound_ShouldThrowDomainException()
    {
        // Arrange
        _orderRepository.Setup(o => o.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Order?)default);

        // Act
        var func = () => _createPaymentUseCase.Execute(Guid.NewGuid());

        // Assert
        using var scope = new AssertionScope();
        await func.Should()
            .ThrowExactlyAsync<EntityNotFoundException>()
            .WithMessage("Order not found.");
        _paymentGatewayMock.Verify(ps => ps.CreatePaymentAsync(It.IsAny<Order>()), Times.Never);
    }

    [Fact]
    public async Task Execute_OrderAlreadyHasPayment_ShouldThrowDomainException()
    {
        // Arrange
        _orderRepository.Setup(o => o.GetAsync(It.IsAny<Guid>()))
            .ReturnsAsync(new Order
            {
                PaymentId = new PaymentId("code", 0)
            });

        // Act
        var func = () => _createPaymentUseCase.Execute(Guid.NewGuid());

        // Assert
        using var scope = new AssertionScope();
        await func.Should()
            .ThrowExactlyAsync<DomainException>()
            .WithMessage("There's already a Payment for the order.");
    }
}
