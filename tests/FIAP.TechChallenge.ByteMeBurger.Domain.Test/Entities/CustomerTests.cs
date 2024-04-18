using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Test.Entities;

public class CustomerTests
{
    private const string ValidCpf = "781.190.140-49";

    [Fact]
    public void Customer_AnonymousCustomer()
    {
        var customer = new Customer();
        
        Assert.False(string.IsNullOrWhiteSpace(customer.Id));
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    public void Customer_InvalidName_ThrowsError(string name)
    {
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
        Assert.Throws<ArgumentException>(() => new Customer(ValidCpf, "customer name", email));
    }
    
    [Theory]
    [InlineData(" ")]
    [InlineData("")]
    [InlineData("123.456.125")]
    [InlineData("000.000.000-00")]
    public void Customer_InvalidCpf_ThrowsError(string cpf)
    {
        Assert.Throws<ArgumentException>(() => new Customer(cpf));
    }

    [Theory]
    [InlineData("781.190.140-49", "joe", "joe@email.com")]
    public void Customer_ValidCustomerInput(string cpf, string name, string email)
    {
        var customer = new Customer(cpf, name, email);
        Assert.Equal(customer.Id, cpf);
        Assert.Equal(customer.Name, name);
        Assert.Equal(customer.Email, email);
    }
}