// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using AutoFixture;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Payment;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;

namespace FIAP.TechChallenge.ByteMeBurger.Application.Test.UseCases.Payment;

[TestSubject(typeof(UpdatePaymentStatusUseCase))]
public class UpdatePaymentStatusUseCaseTest
{
    private readonly Mock<IUpdateOrderStatusUseCase> _mockUpdateOrderStatusUseCase;
    private readonly Mock<IPaymentRepository> _mockPaymentRepository;
    private readonly UpdatePaymentStatusUseCase _target;

    public UpdatePaymentStatusUseCaseTest()
    {
        _mockUpdateOrderStatusUseCase = new Mock<IUpdateOrderStatusUseCase>();
        _mockPaymentRepository = new Mock<IPaymentRepository>();
        _target = new UpdatePaymentStatusUseCase(_mockUpdateOrderStatusUseCase.Object, _mockPaymentRepository.Object);
    }

    [Fact]
    public async void Execute_UpdatePaymentAndOrderStatus_Success()
    {
        // Arrange
        var payment = new Fixture().Create<Domain.Entities.Payment>();
        var newStatus = PaymentStatus.Paid;

        _mockPaymentRepository.Setup(p =>
                p.UpdatePaymentStatusAsync(It.Is<Domain.Entities.Payment>(x => x.Status == newStatus)))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        var result = await _target.Execute(payment, newStatus);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeTrue();
            _mockUpdateOrderStatusUseCase.Verify();
        }
    }
}
