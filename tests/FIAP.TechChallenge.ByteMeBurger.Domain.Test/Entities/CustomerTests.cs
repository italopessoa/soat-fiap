using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

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
        Assert.False(string.IsNullOrWhiteSpace(customer.Id));
        Assert.Equal("Anonymous",customer.Name);
    }
   
    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    public void Customer_InvalidName_ThrowsError(string name)
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<ArgumentException>(() => new Customer(ValidCpf, name, "email@email.com"));
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
        // Assert
        Assert.Throws<ArgumentException>(() => new Customer(ValidCpf, "customer name", email));
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
        // Assert
        Assert.Throws<ArgumentException>(() => new Customer(cpf));
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
    }
}