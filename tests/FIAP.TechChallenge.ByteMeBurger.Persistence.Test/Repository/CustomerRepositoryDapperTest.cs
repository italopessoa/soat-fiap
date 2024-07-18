using System.Data;
using Dapper;
using FIAP.TechChallenge.ByteMeBurger.Domain.Base;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Persistence.Dto;
using FIAP.TechChallenge.ByteMeBurger.Persistence.Repository;
using FluentAssertions;
using FluentAssertions.Execution;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Dapper;

namespace FIAP.TechChallenge.ByteMeBurger.Persistence.Test.Repository;

[TestSubject(typeof(CustomerRepositoryDapper))]
public class CustomerRepositoryDapperTest
{
    private readonly Mock<IDbConnection> _mockDbConnection;
    private readonly CustomerRepositoryDapper _target;
    private const string Cpf = "20697137090";

    public CustomerRepositoryDapperTest()
    {
        Mock<IDbContext> mockDbContext = new();
        _mockDbConnection = new Mock<IDbConnection>();
        _mockDbConnection.Setup(c => c.BeginTransaction()).Returns(Mock.Of<IDbTransaction>());
        mockDbContext.Setup(s => s.CreateConnection())
            .Returns(_mockDbConnection.Object);
        _target = new CustomerRepositoryDapper(mockDbContext.Object, Mock.Of<ILogger<CustomerRepositoryDapper>>());
    }

    [Fact]
    public async Task Create_Success()
    {
        // Arrange
        var customer = new Customer(Cpf);

        _mockDbConnection.SetupDapperAsync(c =>
                c.ExecuteAsync("", null, null, null, null))
            .ReturnsAsync(1);

        // Act
        var result = await _target.CreateAsync(customer);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().Be(customer);
        }
    }

    [Fact]
    public async Task FindByCpf_Success()
    {
        // Arrange
        var expectedCustomer = new CustomerDto()
        {
            Id = Guid.NewGuid(),
            Cpf = Cpf,
            Name = "italo",
            Email = "italo@gmail.com"
        };

        _mockDbConnection.SetupDapperAsync(c =>
                c.QuerySingleOrDefaultAsync<CustomerDto>(It.IsAny<string>(), null, null, null, null))
            .ReturnsAsync(expectedCustomer);

        // Act
        var result = await _target.FindByCpfAsync(Cpf);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(expectedCustomer,
                options => options.ComparingByMembers<CustomerDto>().Excluding(c => c.Cpf));
            result.Cpf.Value.Should().Be(expectedCustomer.Cpf);
        }
    }

    [Fact]
    public async Task FindByCpf_NotFound()
    {
        // Arrange
        _mockDbConnection.SetupDapperAsync(c =>
                c.QuerySingleOrDefaultAsync<Customer>(It.IsAny<string>(), null, null, null, null))
            .ReturnsAsync(default(Customer));

        // Act
        var result = await _target.FindByCpfAsync(Cpf);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeNull();
        }
    }
}
