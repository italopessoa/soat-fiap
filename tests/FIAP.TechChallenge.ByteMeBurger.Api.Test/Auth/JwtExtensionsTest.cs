using System.Security.Claims;
using FIAP.TechChallenge.ByteMeBurger.Api.Auth;
using FluentAssertions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Test.Auth;

[TestSubject(typeof(JwtExtensions))]
public class JwtExtensionsTest
{
    private const string Cpf = "15311546034";

    [Fact]
    public void GetCustomerFromClaims_ShouldReturnCustomerDto_WhenClaimsAreValid()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new(ClaimTypes.Email, "test@example.com"),
            new(ClaimTypes.Name, "Test User"),
            new("cpf", Cpf)
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext { User = claimsPrincipal };

        // Act
        var result = httpContext.GetCustomerFromClaims();

        // Assert
        result.Should().NotBeNull();
        result.Email.Should().Be("test@example.com");
        result.Name.Should().Be("Test User");
        result.Cpf.Should().Be(Cpf);
    }

    [Fact]
    public void GetCustomerFromClaims_ShouldReturnNull_WhenNameIdentifierClaimIsMissing()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, "test@example.com"),
            new(ClaimTypes.Name, "Test User"),
            new("cpf", Cpf)
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext { User = claimsPrincipal };

        // Act
        var result = httpContext.GetCustomerFromClaims();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetCustomerFromClaims_ShouldReturnNull_WhenNameIdentifierClaimIsInvalid()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, "invalid-guid"),
            new(ClaimTypes.Email, "test@example.com"),
            new(ClaimTypes.Name, "Test User"),
            new("cpf", Cpf)
        };
        var identity = new ClaimsIdentity(claims, "TestAuthType");
        var claimsPrincipal = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext { User = claimsPrincipal };

        // Act
        var result = httpContext.GetCustomerFromClaims();

        // Assert
        result.Should().BeNull();
    }
}
