// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;
using FIAP.TechChallenge.ByteMeBurger.Domain.Base;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;

namespace FIAP.TechChallenge.ByteMeBurger.Application.Test.UseCases.Orders;

[TestSubject(typeof(UpdateOrderPaymentUseCase))]
public class UpdateOrderPaymentUseCaseTest
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly UpdateOrderPaymentUseCase _useCase;

    public UpdateOrderPaymentUseCaseTest()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _useCase = new UpdateOrderPaymentUseCase(_orderRepositoryMock.Object);
    }

    [Fact]
    public async void UpdateOrderPayment_OrderNotFound()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        var paymentId = new PaymentId(Guid.NewGuid());
        _orderRepositoryMock.Setup(r => r.GetAsync(orderId)).ReturnsAsync((Order)null);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _useCase.Execute(orderId, paymentId));
    }

    [Fact]
    public async void UpdateOrderPayment_PaymentUpdated()
    {
        // Arrange
        var currentOrder = new Order(Guid.NewGuid(), null, OrderStatus.PaymentPending, new OrderTrackingCode("code"),
            DateTime.UtcNow, null);
        var paymentId = new PaymentId(Guid.NewGuid());
        _orderRepositoryMock.Setup(r => r.GetAsync(currentOrder.Id))
            .ReturnsAsync(currentOrder);

        _orderRepositoryMock.Setup(r => r.UpdateOrderPaymentAsync(It.Is<Order>(o => o.PaymentId == paymentId)))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        var updated = await _useCase.Execute(currentOrder.Id, paymentId);


        // Assert
        updated.Should().BeTrue();
    }
}
