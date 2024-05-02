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
        var product = new Product("product", "description", ProductCategory.Beverage, 10,
            new List<string> { "image1" });
        var sql =
            "INSERT INTO Products (Name, Description, Category, Price, Images) VALUES (@Name, @Description, @Category, @Price, @Images)";
        var parameters = new
        {
            Name = product.Name, Description = product.Description, Category = product.Category.ToString(),
            Price = product.Price, Images = string.Join(",", product.Images)
        };

        _mockConnection.SetupDapperAsync(c => c.ExecuteAsync(sql, parameters, null, null, null))
            .ReturnsAsync(1);

        // Act
        var result = await _target.CreateAsync(product);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(product, options => options.ComparingByMembers<Product>());
            _mockConnection.Verify();
        }
    }

    [Fact]
    public async Task DeleteAsync_Success()
    {
        // Arrange
        var productId = Guid.NewGuid();
        _mockConnection.SetupDapperAsync(db => db.ExecuteAsync(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
            .ReturnsAsync(1);

        // Act
        var result = await _target.DeleteAsync(productId);

        // Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public async Task DeleteAsync_Fail()
    {
        // Arrange
        var productId = Guid.NewGuid();
        _mockConnection.SetupDapperAsync(db => db.ExecuteAsync(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
            .ReturnsAsync(0);

        // Act
        var result = await _target.DeleteAsync(productId);

        // Assert
        result.Should().BeFalse();
    }
}