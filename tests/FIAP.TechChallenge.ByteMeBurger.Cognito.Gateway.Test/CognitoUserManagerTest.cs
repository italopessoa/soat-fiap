using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Bmb.Domain.Core.Base;
using FIAP.TechChallenge.ByteMeBurger.Cognito.Gateway.Factory;
using Bmb.Domain.Core.Entities;
using FluentAssertions;
using FluentAssertions.Execution;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace FIAP.TechChallenge.ByteMeBurger.Cognito.Gateway.Test;

[TestSubject(typeof(CognitoUserManager))]
public class CognitoUserManagerTest
{
    private readonly Mock<IAmazonCognitoIdentityProvider> _cognitoClientMock;
    private readonly CognitoUserManager _userManager;
    private const string Cpf = "28642827041";

    public CognitoUserManagerTest()
    {
        // Arrange
        var mockFactory = new Mock<ICognitoClientFactory>();
        _cognitoClientMock = new Mock<IAmazonCognitoIdentityProvider>();
        var settings = Options.Create(new CognitoSettings
            { UserPoolId = "testPoolId" });

        mockFactory.Setup(f => f.CreateClient()).Returns(_cognitoClientMock.Object);
        _userManager = new CognitoUserManager(mockFactory.Object, Mock.Of<ILogger<CognitoUserManager>>(), settings);
    }

    [Fact]
    public async Task FindByCpfAsync_ShouldReturnCustomer_WhenUserExists()
    {
        // Arrange
        var response = new AdminGetUserResponse
        {
            UserAttributes =
            [
                new AttributeType { Name = "email", Value = "test@example.com" },
                new AttributeType { Name = "name", Value = "Test User" },
                new AttributeType { Name = "sub", Value = Guid.NewGuid().ToString() },
                new AttributeType { Name = "username", Value = Cpf }
            ]
        };

        _cognitoClientMock.Setup(c => c.AdminGetUserAsync(It.IsAny<AdminGetUserRequest>(), default))
            .ReturnsAsync(response);

        // Act
        var result = await _userManager.FindByCpfAsync(Cpf);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Cpf.Value.Should().Be(Cpf);
            result.Email.Should().Be("test@example.com");
            result.Name.Should().Be("Test User");
            result.Id.Should().NotBeEmpty();
        }
    }

    [Fact]
    public async Task FindByCpfAsync_ShouldReturnNull_WhenUserNotFound()
    {
        // Arrange
        _cognitoClientMock.Setup(c => c.AdminGetUserAsync(It.IsAny<AdminGetUserRequest>(), default))
            .ThrowsAsync(new UserNotFoundException("User not found"));

        // Act
        var result = await _userManager.FindByCpfAsync(Cpf);

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

    [Fact]
    public async Task CreateAsync_ShouldThrowDomainException_WhenUsernameExists()
    {
        // Arrange
        var customer = new Customer(Guid.NewGuid(), "28642827041", "Test User", "test@example.com");

        _cognitoClientMock.Setup(c => c.AdminCreateUserAsync(It.IsAny<AdminCreateUserRequest>(), default))
            .ThrowsAsync(new UsernameExistsException("Username already exists"));

        // Act
        Func<Task> act = async () => await _userManager.CreateAsync(customer);

        // Assert
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage("There's already a customer using the provided CPF value.");
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenUnexpectedErrorOccurs()
    {
        // Arrange
        var customer = new Customer(Guid.NewGuid(), "28642827041", "Test User", "test@example.com");
        var exception = new Exception("Unexpected error");

        _cognitoClientMock.Setup(c => c.AdminCreateUserAsync(It.IsAny<AdminCreateUserRequest>(), default))
            .ThrowsAsync(exception);

        // Act
        Func<Task> act = async () => await _userManager.CreateAsync(customer);

        // Assert
        await act.Should().ThrowAsync<Exception>()
            .WithMessage("Unexpected error");
    }

    [Fact]
    public async Task FindByIdAsync_UserExists_ReturnsCustomer()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var userAttributes = new List<AttributeType>
        {
            new() { Name = "email", Value = "test@example.com" },
            new() { Name = "name", Value = "Test User" },
            new() { Name = "sub", Value = userId.ToString() }
        };
        var user = new UserType { Attributes = userAttributes, Username = Cpf };
        var listUsersResponse = new ListUsersResponse { Users = new List<UserType> { user } };

        _cognitoClientMock.Setup(c => c.ListUsersAsync(It.IsAny<ListUsersRequest>(), default))
            .ReturnsAsync(listUsersResponse);

        // Act
        var result = await _userManager.FindByIdAsync(userId);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Id.Should().Be(userId);
            result.Cpf.Value.Should().Be(Cpf);
            result.Name.Should().Be("Test User");
            result.Email.Should().Be("test@example.com");
        }
    }

    [Fact]
    public async Task FindByIdAsync_UserNotFound_ReturnsNull()
    {
        // Arrange
        var userId = Guid.NewGuid();

        _cognitoClientMock.Setup(c => c.ListUsersAsync(It.IsAny<ListUsersRequest>(), default))
            .ThrowsAsync(new UserNotFoundException("User not found"));

        // Act
        var result = await _userManager.FindByIdAsync(userId);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task FindByCpfAsync_ExceptionThrown_LogsErrorAndThrows()
    {
        // Arrange
        var exception = new Exception("Error fetching user");

        _cognitoClientMock.Setup(c => c.AdminGetUserAsync(It.IsAny<AdminGetUserRequest>(), default))
            .ThrowsAsync(exception);

        // Act & Assert
        var func = () => _userManager.FindByCpfAsync(Cpf);
        await func
            .Should()
            .ThrowAsync<Exception>()
            .WithMessage(exception.Message);
    }

    [Fact]
    public async Task FindByIdAsync_ExceptionThrown_LogsErrorAndThrows()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var exception = new Exception("Test exception");

        _cognitoClientMock.Setup(c => c.ListUsersAsync(It.IsAny<ListUsersRequest>(), default))
            .ThrowsAsync(exception);

        // Act & Assert
        var func = () => _userManager.FindByIdAsync(userId);
        await func
            .Should()
            .ThrowAsync<Exception>()
            .WithMessage(exception.Message);
    }
}
