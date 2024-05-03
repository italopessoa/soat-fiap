using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
using FluentAssertions;
using FluentAssertions.Execution;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Test.Entities;

public class CustomerTests
{
    private const string ValidCpf = "781.190.140-49";

    [Fact(Skip = "changing the business rules")]
    public void Customer_AnonymousCustomer()
    {
        // Arrange
        // Act
        var customer = new Customer();

        // Assert
        using (new AssertionScope())
        {
            customer.Id.Should().NotBeEmpty();
            customer.Name.Should().BeNull();
        }
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    public void Customer_InvalidName_ThrowsError(string name)
    {
        // Arrange
        // Act
        var func = () => new Customer(ValidCpf, name, "email@email.com");

        // Assert
        func.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    [InlineData("emailgoogle.com")]
    [InlineData("@google.com")]
    [InlineData("@.com")]
    [InlineData("email@google")]
    public void Customer_InvalidEmail_ThrowsError(string email)
    {
        // Arrange
        // Act
        var func = () => new Customer(ValidCpf, "customer name", email);

        // Assert
        func.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(" ")]
    [InlineData("")]
    [InlineData("123.456.125")]
    [InlineData("000.000.000-00")]
    public void Customer_InvalidId_ThrowsError(string cpf)
    {
        // Arrange
        // Act
        var func = () => new Customer(cpf);

        // Assert
        func.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("78119014049", "joe", "joe@email.com")]
    public void Customer_ValidCustomerInput(string cpf, string name, string email)
    {
        // Arrange
        // Act
        var customer = new Customer(cpf, name, email);

        // Assert
        using (new AssertionScope())
        {
            customer.Id.Should().NotBeEmpty();
            customer.Cpf.Should().Be(new Cpf(cpf));
            customer.Name.Should().Be(name);
            customer.Email.Should().Be(email);
        }
    }
}