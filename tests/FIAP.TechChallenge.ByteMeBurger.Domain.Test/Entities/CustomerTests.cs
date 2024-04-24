using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FluentAssertions;
using FluentAssertions.Execution;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Test.Entities;

public class CustomerTests
{
    private const string ValidCpf = "781.190.140-49";

    [Fact]
    public void Customer_AnonymousCustomer()
    {
        // Arrange
        // Act
        var customer = new Customer();

        // Assert
        using (new AssertionScope())
        {
            customer.Id.Should().NotBeEmpty();
            customer.Name.Should().Be("Anonymous");
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
    [InlineData("9f51d2bb-ee4d-4e3c-a963-7938ce74be3")]
    [InlineData("00000000-0000-0000-0000-000000000000")]
    public void Customer_InvalidId_ThrowsError(string cpf)
    {
        // Arrange
        // Act
        var func = () => new Customer(cpf);

        // Assert
        func.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData("9f51d2bb-ee4d-4e3c-a963-7938ce74be32", "joe", "joe@email.com")]
    [InlineData("78119014049", "joe", "joe@email.com")]
    public void Customer_ValidCustomerInput(string cpf, string name, string email)
    {
        // Arrange
        // Act
        var customer = new Customer(cpf, name, email);

        // Assert
        Assert.Equal(customer.Id, cpf);
        Assert.Equal(customer.Name, name);
        Assert.Equal(customer.Email, email);

        using (new AssertionScope())
        {
            customer.Id.Should().Be(cpf);
            customer.Name.Should().Be(name);
            customer.Email.Should().Be(email);
        }
    }
}