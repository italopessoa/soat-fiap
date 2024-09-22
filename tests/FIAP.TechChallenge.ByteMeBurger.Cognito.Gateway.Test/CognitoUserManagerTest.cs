using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using FIAP.TechChallenge.ByteMeBurger.Cognito.Gateway.Factory;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FluentAssertions;
using FluentAssertions.Execution;
using JetBrains.Annotations;
using Microsoft.Extensions.Options;
using Moq;

namespace FIAP.TechChallenge.ByteMeBurger.Cognito.Gateway.Test;

[TestSubject(typeof(CognitoUserManager))]
public class CognitoUserManagerTest
{
    private readonly Mock<IAmazonCognitoIdentityProvider> _cognitoClientMock;
    private readonly CognitoUserManager _userManager;

    public CognitoUserManagerTest()
    {
        // Arrange
        var mockFactory = new Mock<ICognitoClientFactory>();
        _cognitoClientMock = new Mock<IAmazonCognitoIdentityProvider>();
        var settings = Options.Create(new CognitoSettings
            { UserPoolId = "testPoolId", UserPoolClientId = "testClientId" });

        mockFactory.Setup(f => f.CreateClient()).Returns(_cognitoClientMock.Object);

        _userManager = new CognitoUserManager(mockFactory.Object, settings);
    }

    [Fact]
    public async Task FindByCpfAsync_ShouldReturnCustomer_WhenUserExists()
    {
        // Arrange
        var cpf = "28642827041";
        var response = new AdminGetUserResponse
        {
            UserAttributes =
            [
                new AttributeType { Name = "email", Value = "test@example.com" },
                new AttributeType { Name = "name", Value = "Test User" },
                new AttributeType { Name = "sub", Value = Guid.NewGuid().ToString() }
            ]
        };

        _cognitoClientMock.Setup(c => c.AdminGetUserAsync(It.IsAny<AdminGetUserRequest>(), default))
            .ReturnsAsync(response);

        // Act
        var result = await _userManager.FindByCpfAsync(cpf);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Cpf.Value.Should().Be(cpf);
            result.Email.Should().Be("test@example.com");
            result.Name.Should().Be("Test User");
            result.Id.Should().NotBeEmpty();
        }
    }

    [Fact]
    public async Task FindByCpfAsync_ShouldReturnNull_WhenUserNotFound()
    {
        // Arrange
        const string cpf = "123456789";

        _cognitoClientMock.Setup(c => c.AdminGetUserAsync(It.IsAny<AdminGetUserRequest>(), default))
            .ThrowsAsync(new UserNotFoundException("User not found"));

        // Act
        var result = await _userManager.FindByCpfAsync(cpf);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCustomer_WhenUserIsCreated()
    {
        // Arrange
        var customer = new Customer(Guid.NewGuid(), "28642827041", "Test User", "test@example.com");

        _cognitoClientMock.Setup(c => c.AdminCreateUserAsync(It.IsAny<AdminCreateUserRequest>(), default))
            .ReturnsAsync(new AdminCreateUserResponse
            {
                User = new UserType
                {
                    Attributes = [new AttributeType() { Name = "sub", Value = customer.Id.ToString() }]
                }
            });

        // Act
        var result = await _userManager.CreateAsync(customer);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Cpf.Should().Be(customer.Cpf);
            result.Name.Should().Be(customer.Name);
            result.Email.Should().Be(customer.Email);
            result.Id.Should().NotBeEmpty();
        }
    }
}
