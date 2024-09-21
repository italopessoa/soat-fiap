using Amazon.CognitoIdentityProvider;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Moq;

namespace FIAP.TechChallenge.ByteMeBurger.Cognito.Gateway.Test;

[TestSubject(typeof(CognitoClientFactory))]
public class CognitoClientFactoryTest
{
    [Fact]
    public void CreateClient_ShouldReturnAmazonCognitoIdentityProviderClient()
    {
        // Arrange
        var settingsMock = new Mock<IOptions<CognitoSettings>>();
        settingsMock.Setup(s => s.Value).Returns(new CognitoSettings
        {
            ClientId = "test-client-id",
            ClientSecret = "test-client-secret",
            Region = "us-west-2"
        });

        var factory = new CognitoClientFactory(settingsMock.Object);

        // Act
        var client = factory.CreateClient();

        // Assert
        client.Should().NotBeNull().And.BeOfType<AmazonCognitoIdentityProviderClient>();
    }
}
