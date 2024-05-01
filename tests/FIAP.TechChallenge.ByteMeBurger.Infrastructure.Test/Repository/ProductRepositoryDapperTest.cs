using System.Data;
using Dapper;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
using FIAP.TechChallenge.ByteMeBurger.Infrastructure.Repository;
using FluentAssertions;
using FluentAssertions.Execution;
using JetBrains.Annotations;
using Moq;
using Moq.Dapper;

namespace FIAP.TechChallenge.ByteMeBurger.Infrastructure.Test.Repository;

[TestSubject(typeof(ProductRepositoryDapper))]
public class ProductRepositoryDapperTest
{
    private readonly Mock<IDbConnection> _mockConnection;
    private readonly ProductRepositoryDapper _target;

    public ProductRepositoryDapperTest()
    {
        _mockConnection = new Mock<IDbConnection>();
        _target = new ProductRepositoryDapper(_mockConnection.Object);
    }

    [Fact]
    public async Task Create_Success()
    {
        // Arrange
        var product = new Product("product", "description", ProductCategory.Beverage, 10, ["image1"]);

        _mockConnection.SetupDapperAsync(c => c.ExecuteAsync("", null, null, null, null))
            .ReturnsAsync(1);

        // Act
        var result = await _target.CreateAsync(product);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().Be(product);
        }
    }
}