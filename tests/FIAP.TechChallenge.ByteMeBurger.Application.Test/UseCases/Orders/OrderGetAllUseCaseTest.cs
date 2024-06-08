// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using System.Collections.ObjectModel;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;

namespace FIAP.TechChallenge.ByteMeBurger.Application.Test.UseCases.Orders;

[TestSubject(typeof(OrderGetAllUseCase))]
public class OrderGetAllUseCaseTest
{
    private readonly Mock<IOrderRepository> _orderRepository;
    private readonly IOrderGetAllUseCase _useCase;

    public OrderGetAllUseCaseTest()
    {
        _orderRepository = new Mock<IOrderRepository>();
        _useCase = new OrderGetAllUseCase(_orderRepository.Object);
    }

    [Fact]
    public async Task GetAll_Success()
    {
        // Arrange
        var response = new List<Order>()
        {
            new (Guid.NewGuid(),null, OrderStatus.InPreparation, null, DateTime.Now, null),
            new (Guid.NewGuid(),null, OrderStatus.Ready, null, DateTime.Now, null),
            new (Guid.NewGuid(),null, OrderStatus.Received, null, DateTime.Now, null),
            new (Guid.NewGuid(),null, OrderStatus.PaymentPending, null, DateTime.Now, null),
            new (Guid.NewGuid(),null, OrderStatus.PaymentConfirmed, null, DateTime.Now, null),
            new (Guid.NewGuid(),null, OrderStatus.Completed, null, DateTime.Now, null),
        }.AsReadOnly();

        var expectedOrders = new List<Order>()
        {
            response[1],
            response[0],
            response[2],
        }.AsReadOnly();

        _orderRepository.Setup(r => r.GetAllAsync())
            .ReturnsAsync(response.AsReadOnly);

        // Act
        var result = await _useCase.Execute();

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.Should().BeEquivalentTo(expectedOrders, options => options.WithStrictOrdering());
            _orderRepository.Verify(m => m.GetAllAsync(), Times.Once);
        }
    }

    [Fact]
    public async Task GetAll_Empty()
    {
        // Arrange
        _orderRepository.Setup(r => r.GetAllAsync())
            .ReturnsAsync((ReadOnlyCollection<Order>)default!);

        // Act
        var result = await _useCase.Execute();

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().BeEmpty();
            _orderRepository.Verify(m => m.GetAllAsync(), Times.Once);
        }
    }
}
