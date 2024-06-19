// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using AutoFixture;
using FIAP.TechChallenge.ByteMeBurger.Application.Services;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Payment;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;

namespace FIAP.TechChallenge.ByteMeBurger.Application.Test.Services;

[TestSubject(typeof(PaymentService))]
public class PaymentServiceTests
{
    private readonly Mock<ICreatePaymentUseCase> _mockCreatePaymentUseCase;
    private readonly Mock<IPaymentRepository> _mockPaymentRepository;
    private readonly PaymentService _target;

    public PaymentServiceTests()
    {
        _mockCreatePaymentUseCase = new Mock<ICreatePaymentUseCase>();
        _mockPaymentRepository = new Mock<IPaymentRepository>();
        _target = new PaymentService(_mockCreatePaymentUseCase.Object, _mockPaymentRepository.Object);
    }

    [Fact]
    public async Task CreateOrderPaymentAsync_Success()
    {
        // Arrange
        _mockCreatePaymentUseCase.Setup(p => p.Execute(It.IsAny<Guid>()))
            .ReturnsAsync(new Fixture().Create<Payment>());

        // Act
        var result = await _target.CreateOrderPaymentAsync(Guid.NewGuid());

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            _mockPaymentRepository.Verify(r => r.SaveAsync(It.IsAny<Payment>()), Times.Once);
        }
    }

    [Fact]
    public async Task GetPaymentAsync_Success()
    {
        // Arrange
        var expectedPayment = new Fixture().Create<Payment>();
        _mockPaymentRepository.Setup(p => p.GetPaymentAsync(It.IsAny<string>()))
            .ReturnsAsync(expectedPayment)
            .Verifiable();

        // Act
        var result = await _target.GetPaymentAsync("paymentId");

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().Be(expectedPayment);
            _mockPaymentRepository.Verify();
        }
    }
}
