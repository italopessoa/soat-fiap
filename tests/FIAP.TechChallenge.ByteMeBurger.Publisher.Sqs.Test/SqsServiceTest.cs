using Amazon.SQS;
using Amazon.SQS.Model;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Events;
using FIAP.TechChallenge.ByteMeBurger.Publisher.Sqs.Factory;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace FIAP.TechChallenge.ByteMeBurger.Publisher.Sqs.Test;

[TestSubject(typeof(SqsService))]
public class SqsServiceTests
{
    private readonly Mock<ISqsClientFactory> _sqsFactoryMock;
    private readonly Mock<IOptions<SqsSettings>> _sqsSettingsOptionsMock;
    private readonly SqsService _sqsService;

    public SqsServiceTests()
    {
        _sqsFactoryMock = new Mock<ISqsClientFactory>();
        _sqsSettingsOptionsMock = new Mock<IOptions<SqsSettings>>();
        _sqsService = new SqsService(_sqsFactoryMock.Object, _sqsSettingsOptionsMock.Object);
    }

    [Fact]
    public async Task PublishAsync_ShouldSendMessage_WhenEnabled()
    {
        // Arrange
        var sqsSettings = new SqsSettings { Enabled = true, QueueName = "test-queue" };
        _sqsSettingsOptionsMock.Setup(x => x.Value).Returns(sqsSettings);

        var sqsClientMock = new Mock<IAmazonSQS>();
        _sqsFactoryMock.Setup(x => x.CreateClient()).Returns(sqsClientMock.Object);
        sqsClientMock.Setup(x => x.GetQueueUrlAsync(It.IsAny<string>(), default))
            .ReturnsAsync(new GetQueueUrlResponse { QueueUrl = "test-queue-url" });
        sqsClientMock.Setup(x => x.SendMessageAsync(It.IsAny<SendMessageRequest>(), default))
            .ReturnsAsync(new SendMessageResponse());

        var domainEvent = new OrderCreated(new Order());

        // Act
        await _sqsService.PublishAsync(domainEvent);

        // Assert
        sqsClientMock.Verify(x => x.SendMessageAsync(It.IsAny<SendMessageRequest>(), default), Times.Once);
    }

    [Fact]
    public async Task PublishAsync_ShouldNotSendMessage_WhenDisabled()
    {
        // Arrange
        var sqsSettings = new SqsSettings { Enabled = false };
        _sqsSettingsOptionsMock.Setup(x => x.Value).Returns(sqsSettings);

        var domainEvent = new OrderCreated(new Order());

        // Act
        await _sqsService.PublishAsync(domainEvent);

        // Assert
        _sqsFactoryMock.Verify(x => x.CreateClient(), Times.Never);
    }

    [Fact]
    public async Task PublishAsync_ShouldThrowException_WhenSendMessageFails()
    {
        // Arrange
        var sqsSettings = new SqsSettings { Enabled = true, QueueName = "test-queue" };
        _sqsSettingsOptionsMock.Setup(x => x.Value).Returns(sqsSettings);

        var sqsClientMock = new Mock<IAmazonSQS>();
        _sqsFactoryMock.Setup(x => x.CreateClient()).Returns(sqsClientMock.Object);
        sqsClientMock.Setup(x => x.GetQueueUrlAsync(It.IsAny<string>(), default))
            .ReturnsAsync(new GetQueueUrlResponse { QueueUrl = "test-queue-url" });
        sqsClientMock.Setup(x => x.SendMessageAsync(It.IsAny<SendMessageRequest>(), default))
            .ThrowsAsync(new Exception("Test exception"));

        var domainEvent = new OrderCreated(new Order());
        var func = () => _sqsService!.PublishAsync(domainEvent);

        // Assert
        await func.Should().ThrowAsync<Exception>();
    }
}
