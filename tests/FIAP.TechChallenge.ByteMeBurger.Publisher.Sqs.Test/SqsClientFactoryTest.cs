using Amazon;
using Amazon.SQS;
using FluentAssertions;
using FluentAssertions.Execution;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace FIAP.TechChallenge.ByteMeBurger.Publisher.Sqs.Test;

[TestSubject(typeof(SqsClientFactory))]
public class SqsClientFactoryTests
{
    private readonly Mock<IOptions<SqsSettings>> _sqsSettingsOptionsMock;
    private readonly SqsClientFactory _sqsClientFactory;

    public SqsClientFactoryTests()
    {
        _sqsSettingsOptionsMock = new Mock<IOptions<SqsSettings>>();
        _sqsClientFactory = new SqsClientFactory(_sqsSettingsOptionsMock.Object);
    }

    [Fact]
    public void CreateClient_ShouldReturnAmazonSQSClient()
    {
        // Arrange
        var region = "us-east-1";
        var sqsSettings = new SqsSettings { Region = region };
        _sqsSettingsOptionsMock.Setup(x => x.Value).Returns(sqsSettings);

        // Act
        var client = _sqsClientFactory.CreateClient();

        // Assert
        using (new AssertionScope())
        {
            client
                .Should()
                .NotBeNull()
                .And
                .BeOfType<AmazonSQSClient>();

            client.Config.RegionEndpoint.Should().Be(RegionEndpoint.GetBySystemName(region));
        }
    }
}
