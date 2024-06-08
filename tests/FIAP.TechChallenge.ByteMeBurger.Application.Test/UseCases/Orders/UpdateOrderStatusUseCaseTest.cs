// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;
using FIAP.TechChallenge.ByteMeBurger.Domain.Base;

namespace FIAP.TechChallenge.ByteMeBurger.Application.Test.UseCases.Orders;

[TestSubject(typeof(UpdateOrderStatusUseCase))]
public class UpdateOrderStatusUseCaseTest
{
    private readonly Mock<IOrderRepository> _orderRepositoryMock;
    private readonly UpdateOrderStatusUseCase _useCase;

    public UpdateOrderStatusUseCaseTest()
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _useCase = new UpdateOrderStatusUseCase(_orderRepositoryMock.Object);
    }

    [Theory]
    [InlineData(OrderStatus.PaymentPending, OrderStatus.PaymentConfirmed)]
    [InlineData(OrderStatus.PaymentConfirmed, OrderStatus.Received)]
    [InlineData(OrderStatus.Received, OrderStatus.InPreparation)]
    [InlineData(OrderStatus.InPreparation, OrderStatus.Ready)]
    [InlineData(OrderStatus.Ready, OrderStatus.Completed)]
    public async Task Execute_ShouldUpdateOrderStatus_WhenOrderExists(OrderStatus currentStatus, OrderStatus newStatus)
    {
        // Arrange
        var currentOrder = new Order(Guid.NewGuid(), null, currentStatus, "", DateTime.UtcNow, null);
        _orderRepositoryMock.Setup(r => r.GetAsync(currentOrder.Id)).ReturnsAsync(currentOrder);

        // Act
        var updatedOrder = await _useCase.Execute(currentOrder.Id, newStatus);

        // Assert
        _orderRepositoryMock.Verify(r => r.GetAsync(It.Is<Guid>(orderId => orderId == currentOrder.Id)),
            Times.Once);

        updatedOrder.Status.Should().Be(newStatus);
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenOrderDoesNotExist()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        _orderRepositoryMock.Setup(r => r.GetAsync(orderId)).ReturnsAsync((Order)null);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _useCase.Execute(orderId, OrderStatus.Completed));
    }

    [Fact]
    public async Task Execute_ShouldThrowException_WhenOrderIsAlreadyCompleted()
    {
        // Arrange
        var orderId = Guid.NewGuid();
        _orderRepositoryMock.Setup(r => r.GetAsync(orderId))
            .ReturnsAsync(new Order(Guid.NewGuid(), null, OrderStatus.Completed, "", DateTime.UtcNow, null));

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => _useCase.Execute(orderId, OrderStatus.Completed));
    }
}
