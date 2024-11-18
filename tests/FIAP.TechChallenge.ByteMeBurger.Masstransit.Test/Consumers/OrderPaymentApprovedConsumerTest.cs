using FIAP.TechChallenge.ByteMeBurger.Masstransit.Consumers;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using FluentAssertions;
using Bmb.Domain.Core.Events.Notifications;
using Bmb.Domain.Core.Interfaces;
using Bmb.Domain.Core.ValueObjects;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;
using JetBrains.Annotations;
using MassTransit;

namespace FIAP.TechChallenge.ByteMeBurger.Masstransit.Test.Consumers;

[TestSubject(typeof(OrderPaymentApprovedConsumer))]
public class OrderPaymentApprovedConsumerTest
{
    private Mock<ILogger<OrderPaymentApprovedConsumer>> _loggerMock;
    private Mock<IUseCase<UpdateOrderStatusRequest, bool>> _useCaseMock;
    private OrderPaymentApprovedConsumer _consumer;

    public OrderPaymentApprovedConsumerTest()
    {
        _loggerMock = new Mock<ILogger<OrderPaymentApprovedConsumer>>();
        _useCaseMock = new Mock<IUseCase<UpdateOrderStatusRequest, bool>>();
        _consumer = new OrderPaymentApprovedConsumer(_loggerMock.Object, _useCaseMock.Object);
    }

    [Fact]
    public async Task Consume_ShouldLogInformation_WhenEventIsProcessed()
    {
        // Arrange
        var contextMock = new Mock<ConsumeContext<OrderPaymentConfirmed>>();
        var orderPaymentConfirmed = new OrderPaymentConfirmed(new PaymentId(Guid.NewGuid()), Guid.NewGuid());
        contextMock.Setup(c => c.Message).Returns(orderPaymentConfirmed);

        // Act
        await _consumer.Consume(contextMock.Object);

        // Assert
        _loggerMock.VerifyLog(logger => logger.LogInformation(It.IsAny<string>(), It.IsAny<object[]>()),
            LogLevel.Information, Times.Exactly(2));
    }

    [Fact]
    public async Task Consume_ShouldCallUseCaseExecuteAsync_WhenEventIsProcessed()
    {
        // Arrange
        var contextMock = new Mock<ConsumeContext<OrderPaymentConfirmed>>();
        var orderPaymentConfirmed = new OrderPaymentConfirmed(new PaymentId(Guid.NewGuid()), Guid.NewGuid());
        contextMock.Setup(c => c.Message).Returns(orderPaymentConfirmed);

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
        var contextMock = new Mock<ConsumeContext<OrderPaymentConfirmed>>();
        var orderPaymentConfirmed = new OrderPaymentConfirmed(new PaymentId(Guid.NewGuid()), Guid.NewGuid());
        contextMock.Setup(c => c.Message).Returns(orderPaymentConfirmed);
        var exception = new Exception("Unexpected error");
        _useCaseMock.Setup(x => x.ExecuteAsync(It.IsAny<UpdateOrderStatusRequest>())).ThrowsAsync(exception);

        // Act
        Func<Task> act = async () => await _consumer.Consume(contextMock.Object);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Unexpected error");
        _loggerMock.VerifyLog(logger => logger.LogError(exception, It.IsAny<string>(), It.IsAny<object[]>()),
            LogLevel.Error, Times.Once());
    }
}
