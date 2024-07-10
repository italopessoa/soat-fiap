using FIAP.TechChallenge.ByteMeBurger.Application.Controllers;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Customers;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;

namespace FIAP.TechChallenge.ByteMeBurger.Application.Test.Services;

public class CustomerServiceTests
{
    private readonly ICustomerService _service;
    private readonly Cpf ValidCpf = new Cpf("863.917.790-23");
    private readonly Mock<ICreateCustomerUseCase> _mockCreateCustomerUseCase;
    private readonly Mock<IFindCustomerByCpfUseCase> _mockFindCustomerByCpfUseCase;

    public CustomerServiceTests()
    {
        _mockCreateCustomerUseCase = new Mock<ICreateCustomerUseCase>();
        _mockFindCustomerByCpfUseCase = new Mock<IFindCustomerByCpfUseCase>();
        _service = new CustomerService(_mockCreateCustomerUseCase.Object, _mockFindCustomerByCpfUseCase.Object);
    }

    [Fact]
    public async Task FindBy_Cpf_Success()
    {
        // Arrange
        var expectedCustomer = new Customer(ValidCpf, "name", "email@email.com");
        _mockFindCustomerByCpfUseCase.Setup(r => r.Execute(ValidCpf))
            .ReturnsAsync(expectedCustomer);

        // Act
        var customer = await _service.FindByCpfAsync(ValidCpf);

        // Assert
        using (new AssertionScope())
        {
            customer.Should().NotBeNull();
            customer.Should().BeEquivalentTo(expectedCustomer, options => options.ComparingByMembers<Customer>());
        }
    }

    [Fact]
    public async Task FindBy_Cpf_NotFound()
    {
        // Arrange
        _mockFindCustomerByCpfUseCase.Setup(r => r.Execute(ValidCpf))
            .ReturnsAsync(default(Customer));

        // Act
        var customer = await _service.FindByCpfAsync("628.040.140-53");

        // Assert
        customer.Should().BeNull();
    }

    [Fact]
    public async Task Create_Customer_CpfOnly_Success()
    {
        // Arrange
        var expectedCustomer = new Customer(ValidCpf);

        _mockCreateCustomerUseCase.Setup(r => r.Execute(It.IsAny<Cpf>(), It.IsAny<string?>(), It.IsAny<string?>()))
            .ReturnsAsync(expectedCustomer);

        // Act
        var result = await _service.CreateAsync(ValidCpf, null, null);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedCustomer, options => options.ComparingByMembers<Customer>());
            result.Id.Should().NotBeEmpty();
        }
    }

    [Theory]
    [InlineData("310.686.640-37", "customer", "email@email.com")]
    public async Task Create_Customer_NameEmail_Success(string cpf, string name, string email)
    {
        // Arrange
        var customer = new Customer(cpf, name, email);
        _mockCreateCustomerUseCase.Setup(r =>
                r.Execute(It.Is<Cpf>(c => c.Equals(customer.Cpf)), It.IsAny<string?>(), It.IsAny<string?>()))
            .ReturnsAsync(customer);

        // Act
        var result = await _service.CreateAsync(cpf, name, email);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().Be(customer);
        }
    }
}