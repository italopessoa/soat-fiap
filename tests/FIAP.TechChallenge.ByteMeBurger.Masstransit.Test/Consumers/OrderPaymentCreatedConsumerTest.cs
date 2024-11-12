using FIAP.TechChallenge.ByteMeBurger.Masstransit.Consumers;
using Moq;
using Xunit;
using FluentAssertions;
using Bmb.Domain.Core.Events.Notifications;
using Bmb.Domain.Core.ValueObjects;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;
using MassTransit;
using JetBrains.Annotations;

namespace FIAP.TechChallenge.ByteMeBurger.Masstransit.Test.Consumers;

[TestSubject(typeof(OrderPaymentCreatedConsumer))]
public class OrderPaymentCreatedConsumerTest
{
    private Mock<IUpdateOrderPaymentUseCase> _useCaseMock;
    private OrderPaymentCreatedConsumer _consumer;

    public OrderPaymentCreatedConsumerTest()
    {
        _useCaseMock = new Mock<IUpdateOrderPaymentUseCase>();
        _consumer = new OrderPaymentCreatedConsumer(_useCaseMock.Object);
    }

    [Fact]
    public async Task Consume_ShouldCallUseCaseExecute_WhenEventIsProcessed()
    {
        // Arrange
        var contextMock = new Mock<ConsumeContext<PaymentCreated>>();
        var paymentCreated = new PaymentCreated(Guid.NewGuid(), Guid.NewGuid());
        contextMock.Setup(c => c.Message).Returns(paymentCreated);

        // Act
        await _consumer.Consume(contextMock.Object);

        // Assert
        _useCaseMock.Verify(
            x => x.Execute(It.IsAny<Guid>(), It.IsAny<PaymentId>()),
            Times.Once);
    }

    [Fact]
    public async Task Consume_ShouldThrowException_WhenUseCaseThrowsException()
    {
        // Arrange
        var contextMock = new Mock<ConsumeContext<PaymentCreated>>();
        var paymentCreated = new PaymentCreated(Guid.NewGuid(), Guid.NewGuid());
        contextMock.Setup(c => c.Message).Returns(paymentCreated);
        var exception = new Exception("Unexpected error");
        _useCaseMock.Setup(x => x.Execute(It.IsAny<Guid>(), It.IsAny<PaymentId>())).Throws(exception);

        // Act
        Func<Task> act = async () => await _consumer.Consume(contextMock.Object);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Unexpected error");
    }
}
