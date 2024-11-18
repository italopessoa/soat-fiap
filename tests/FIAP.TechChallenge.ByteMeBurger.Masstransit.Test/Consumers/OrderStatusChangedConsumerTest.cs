using Bmb.Domain.Core.Events.Notifications;
using Bmb.Domain.Core.Interfaces;
using Bmb.Domain.Core.ValueObjects;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;
using FIAP.TechChallenge.ByteMeBurger.Masstransit.Consumers;
using FluentAssertions;
using JetBrains.Annotations;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace FIAP.TechChallenge.ByteMeBurger.Masstransit.Test.Consumers;

[TestSubject(typeof(OrderStatusChangedConsumer))]
public class OrderStatusChangedConsumerTest
{
    private Mock<ILogger<OrderStatusChangedConsumer>> _loggerMock;
    private Mock<IUseCase<UpdateOrderStatusRequest, bool>> _useCaseMock;
    private OrderStatusChangedConsumer _consumer;

    public OrderStatusChangedConsumerTest()
    {
        _loggerMock = new Mock<ILogger<OrderStatusChangedConsumer>>();
        _useCaseMock = new Mock<IUseCase<UpdateOrderStatusRequest, bool>>();
        _consumer = new OrderStatusChangedConsumer(_loggerMock.Object, _useCaseMock.Object);
    }

    [Fact]
    public async Task Consume_ShouldLogInformation_WhenEventIsProcessed()
    {
        // Arrange
        var contextMock = new Mock<ConsumeContext<OrderStatusChanged>>();
        var orderStatusChanged = new OrderStatusChanged(Guid.NewGuid(), OrderStatus.Completed);
        contextMock.Setup(c => c.Message).Returns(orderStatusChanged);

        // Act
        await _consumer.Consume(contextMock.Object);

        // Assert
        _loggerMock.VerifyLog(logger => logger.LogInformation(It.IsAny<string>(), It.IsAny<object[]>()), LogLevel.Information, Times.Once());
    }

    [Fact]
    public async Task Consume_ShouldCallUseCaseExecuteAsync_WhenEventIsProcessed()
    {
        // Arrange
        var contextMock = new Mock<ConsumeContext<OrderStatusChanged>>();
        var orderStatusChanged = new OrderStatusChanged(Guid.NewGuid(), OrderStatus.Completed);
        contextMock.Setup(c => c.Message).Returns(orderStatusChanged);

        // Act
        await _consumer.Consume(contextMock.Object);

        // Assert
        _useCaseMock.Verify(
            x => x.ExecuteAsync(It.IsAny<UpdateOrderStatusRequest>()),
            Times.Once);
    }

    [Fact]
    public async Task Consume_ShouldLogError_WhenExceptionIsThrown()
    {
        // Arrange
        var contextMock = new Mock<ConsumeContext<OrderStatusChanged>>();
        var orderStatusChanged = new OrderStatusChanged(Guid.NewGuid(), OrderStatus.Completed);
        contextMock.Setup(c => c.Message).Returns(orderStatusChanged);
        var exception = new Exception("Unexpected error");
        _useCaseMock.Setup(x => x.ExecuteAsync(It.IsAny<UpdateOrderStatusRequest>())).ThrowsAsync(exception);

        // Act
        Func<Task> act = async () => await _consumer.Consume(contextMock.Object);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Unexpected error");
        _loggerMock.VerifyLog(logger => logger.LogError(exception, It.IsAny<string>(), It.IsAny<string>()), LogLevel.Error, Times.Once());
    }
}
