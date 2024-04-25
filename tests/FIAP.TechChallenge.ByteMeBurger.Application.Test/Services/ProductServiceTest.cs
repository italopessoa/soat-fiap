using System.Collections.ObjectModel;
using AutoFixture;
using AutoFixture.Xunit2;
using FIAP.TechChallenge.ByteMeBurger.Application.Services;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Outgoing;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
using FluentAssertions;
using FluentAssertions.Execution;
using JetBrains.Annotations;
using Moq;

namespace FIAP.TechChallenge.ByteMeBurger.Application.Test.Services;

[TestSubject(typeof(ProductService))]
public class ProductServiceTest
{
    private readonly Mock<IProductRepository> _mockRepository;
    private readonly ProductService _target;
    private static readonly string[] images = new[] { "images" };

    public ProductServiceTest()
    {
        _mockRepository = new Mock<IProductRepository>();
        _target = new ProductService(_mockRepository.Object);
    }

    [Theory]
    [InlineAutoData]
    public async Task Create_Product_Success(string name, string description, ProductCategory category, decimal price,
        IReadOnlyList<string> images)
    {
        // Arrange
        var expectedProduct = new Product(name, description, category, price, images);
        expectedProduct.Create();

        _mockRepository.Setup(s => s.CreateAsync(It.IsAny<Product>()))
            .ReturnsAsync(expectedProduct);

        // Act
        var product = await _target.CreateAsync(name, description, category, price, images);

        // Assert
        using (new AssertionScope())
        {
            product.Should().NotBeNull();
            product.Id.Should().Be(expectedProduct.Id);
            product.Name.Should().Be(expectedProduct.Name);
            product.Description.Should().Be(expectedProduct.Description);
            product.Category.Should().Be(category);
            product.Price.Should().Be(price);
            product.Images.Should().BeEquivalentTo(images);
            product.CreationDate.Should().NotBe(default);
            product.LastUpdate.Should().BeNull();
        }

        _mockRepository.VerifyAll();
    }

    [Fact]
    public async Task Delete_Product_NotFound()
    {
        // Arrange
        _mockRepository.Setup(s => s.DeleteAsync(It.IsAny<Guid>()))
            .ReturnsAsync(false);

        // Act
        var deleted = await _target.DeleteAsync(Guid.NewGuid());

        // Assert
        deleted.Should().BeFalse();
        _mockRepository.VerifyAll();
    }

    [Fact]
    public async Task Delete_Product_Success()
    {
        // Arrange
        _mockRepository.Setup(s => s.DeleteAsync(It.IsAny<Guid>()))
            .ReturnsAsync(true);

        // Act
        var deleted = await _target.DeleteAsync(Guid.NewGuid());

        // Assert
        deleted.Should().BeTrue();
        _mockRepository.VerifyAll();
    }

    [Fact]
    public async Task GetAll_Empty()
    {
        // Arrange
        _mockRepository.Setup(s => s.GetAll())
            .ReturnsAsync((ReadOnlyCollection<Product>)default!);

        // Act
        var products = await _target.GetAll();

        // Assert
        using (new AssertionScope())
        {
            products.Should().NotBeNull();
            products.Should().BeEmpty();
            _mockRepository.VerifyAll();
        }
    }

    [Fact]
    public async Task GetAll_Success()
    {
        // Arrange
        var expectedProducts = new Fixture().CreateMany<Product>().ToList();
        _mockRepository.Setup(s => s.GetAll())
            .ReturnsAsync(expectedProducts.ToList().AsReadOnly());

        // Act
        var products = await _target.GetAll();

        // Assert
        using (new AssertionScope())
        {
            products.Should().NotBeNull();
            products.Should().BeEquivalentTo(expectedProducts);
            _mockRepository.VerifyAll();
        }
    }

    [Fact]
    public async Task FindByCategory_NotFound()
    {
        // Arrange
        _mockRepository.Setup(s => s.FindByCategory(It.IsAny<ProductCategory>()))
            .ReturnsAsync((ReadOnlyCollection<Product>)null!);

        // Act
        var products = await _target.FindByCategory(ProductCategory.Beverage);

        // Assert
        using (new AssertionScope())
        {
            products.Should().NotBeNull();
            products.Should().BeEmpty();
            _mockRepository.VerifyAll();
        }
    }

    [Theory]
    [InlineAutoData]
    public async Task FindByCategory_Success(ProductCategory category)
    {
        // Arrange
        var expectedProducts = new List<Product>()
        {
            new Product(Guid.NewGuid(), "product", "description", category, 10m, images)
        };

        _mockRepository.Setup(s => s.FindByCategory(It.Is<ProductCategory>(c => c == category)))
            .ReturnsAsync(expectedProducts.AsReadOnly);

        // Act
        var products = await _target.FindByCategory(category);

        // Assert
        using (new AssertionScope())
        {
            products.Should().BeEquivalentTo(expectedProducts);
            _mockRepository.VerifyAll();
        }
    }

    [Fact]
    public async Task Update_NotFound()
    {
        // Arrange
        var product = new Fixture().Create<Product>();

        _mockRepository.Setup(s => s.FindByIdAsync(
                It.IsAny<Guid>()))
            .ReturnsAsync((Product?)null);

        // Act
        var updated = await _target.UpdateAsync(product.Id, product.Name, product.Description,
            ProductCategory.Beverage, product.Price, product.Images);

        // Assert
        using (new AssertionScope())
        {
            updated.Should().BeFalse();
            _mockRepository.Verify(m => m.UpdateAsync(It.IsAny<Product>()), Times.Never);
            _mockRepository.VerifyAll();
        }
    }

    [Fact]
    public async Task Update_Fail()
    {
        // Arrange
        var product = new Fixture().Create<Product>();

        _mockRepository.Setup(s => s.FindByIdAsync(
                It.IsAny<Guid>()))
            .ReturnsAsync(product);

        _mockRepository.Setup(s => s.UpdateAsync(
                It.IsAny<Product>()))
            .ReturnsAsync(false);

        // Act
        var updated = await _target.UpdateAsync(product.Id, product.Name, product.Description,
            ProductCategory.Beverage, product.Price, product.Images);

        // Assert
        using (new AssertionScope())
        {
            updated.Should().BeFalse();
            _mockRepository.VerifyAll();
        }
    }

    [Fact]
    public async Task Update_Success()
    {
        // Arrange
        var product = new Fixture().Create<Product>();

        _mockRepository.Setup(s => s.FindByIdAsync(
                It.IsAny<Guid>()))
            .ReturnsAsync(product);

        _mockRepository.Setup(s => s.UpdateAsync(
                It.IsAny<Product>()))
            .ReturnsAsync(true);

        // Act
        var updated = await _target.UpdateAsync(product.Id, product.Name, product.Description,
            ProductCategory.Beverage, product.Price, product.Images);

        // Assert
        using (new AssertionScope())
        {
            updated.Should().BeTrue();
            _mockRepository.VerifyAll();
        }
    }
}