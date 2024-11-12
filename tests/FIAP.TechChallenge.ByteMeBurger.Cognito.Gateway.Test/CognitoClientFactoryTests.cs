using Amazon.CognitoIdentityProvider;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Moq;

namespace FIAP.TechChallenge.ByteMeBurger.Cognito.Gateway.Test;

[TestSubject(typeof(CognitoClientFactory))]
public class CognitoClientFactoryTests
{
    [Fact]
    public void CreateClient_WithValidSettings_ShouldReturnAmazonCognitoIdentityProviderClient ()
    {
        // Arrange
        var settingsMock = new Mock<IOptions<CognitoSettings>>();
        settingsMock.Setup(s => s.Value).Returns(new CognitoSettings());

        var factory = new CognitoClientFactory();

        // Act
        var client = factory.CreateClient();

        // Assert
        client.Should().NotBeNull().And.BeOfType<AmazonCognitoIdentityProviderClient>();
    }
}
