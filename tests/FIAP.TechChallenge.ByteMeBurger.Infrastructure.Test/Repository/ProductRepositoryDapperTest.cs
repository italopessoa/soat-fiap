using System.Data;
using Dapper;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
using FIAP.TechChallenge.ByteMeBurger.Infrastructure.Dto;
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
        var parameters = (ProductDto)product;
        const string sql = "INSERT INTO Products (Name, Description, Category, Price, Images) VALUES (@Name, @Description, @Category, @Price, @Images)";

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
        _mockConnection
            .SetupDapperAsync(db => db.ExecuteAsync(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
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
        _mockConnection
            .SetupDapperAsync(db => db.ExecuteAsync(It.IsAny<string>(), It.IsAny<object>(), null, null, null))
            .ReturnsAsync(0);

        // Act
        var result = await _target.DeleteAsync(productId);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public async Task FindByIdAsync_Success()
    {
        // Arrange
        var product = new ProductDto
        {
            Id = Guid.NewGuid(),
            Name = "COCA",
            Description = "COCA COLA",
            Category = (int)ProductCategory.Beverage,
            Price = 10,
            Images = "image1|image 2"
        };

        _mockConnection.SetupDapperAsync(c => c.QueryAsync<ProductDto>(It.IsAny<string>(), null, null, null, null))
            .ReturnsAsync([product]);

        // Act
        var result = await _target.FindByCategory(ProductCategory.Beverage);

        // Assert
        using (new AssertionScope())
        {
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.Should().HaveCount(1);
            var newProduct = result.First();
            newProduct.Should().BeEquivalentTo(product, options =>
                options.Excluding(p => p.Images)
                    .Excluding(p => p.Category)
            );
            newProduct.Images.Should().BeEquivalentTo(product.Images.Split("|"));
            newProduct.Category.Should().Be((ProductCategory)product.Category);
        }
    }

    [Fact]
    public async Task Update_Success()
    {
        // Arrange
        var product = new Product("product", "description", ProductCategory.Beverage, 10,
            new List<string> { "image1" });
        const string sql = "UPDATE Products SET Name=@Name, Description=@Description, Category=@Category, Price=@Price, Images=@Images WHERE Id = @Id";
        var parameters = new
        {
            product.Id,
            product.Name,
            product.Description,
            Category = (int)product.Category,
            product.Price,
            Images = string.Join("|", product.Images)
        };

        _mockConnection.SetupDapperAsync(c => c.ExecuteAsync(sql, parameters, null, null, null))
            .ReturnsAsync(1);

        // Act
        var result = await _target.UpdateAsync(product);

        // Assert
        using (new AssertionScope())
        {
            result.Should().BeTrue();
            _mockConnection.Verify();
        }
    }
}