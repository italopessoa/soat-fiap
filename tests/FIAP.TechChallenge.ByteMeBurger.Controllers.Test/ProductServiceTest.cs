using AutoFixture;
using AutoFixture.Xunit2;
using Bmb.Domain.Core.Entities;
using Bmb.Domain.Core.ValueObjects;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Products;
using FIAP.TechChallenge.ByteMeBurger.Controllers.Dto;
using FIAP.TechChallenge.ByteMeBurger.Test.Common;
using FluentAssertions;
using FluentAssertions.Execution;
using JetBrains.Annotations;
using Moq;

namespace FIAP.TechChallenge.ByteMeBurger.Controllers.Test;

[TestSubject(typeof(ProductService))]
public class ProductServiceTest
{
    private readonly Mock<IGetAllProductsUseCase> _mockGetAllProductsUseCase;
    private readonly Mock<IDeleteProductUseCase> _mockDeleteProductUseCase;
    private readonly Mock<IFindProductsByCategoryUseCase> _mockFindProductsByCategoryUseCase;
    private readonly Mock<ICreateProductUseCase> _mockCreateProductUseCase;
    private readonly Mock<IUpdateProductUseCase> _mockUpdateProductUseCase;

    private readonly ProductService _target;
    private static readonly string[] images = new[] { "images" };

    public ProductServiceTest()
    {
        _mockGetAllProductsUseCase = new Mock<IGetAllProductsUseCase>();
        _mockDeleteProductUseCase = new Mock<IDeleteProductUseCase>();
        _mockFindProductsByCategoryUseCase = new Mock<IFindProductsByCategoryUseCase>();
        _mockCreateProductUseCase = new Mock<ICreateProductUseCase>();
        _mockUpdateProductUseCase = new Mock<IUpdateProductUseCase>();

        _target = new ProductService(
            _mockGetAllProductsUseCase.Object,
            _mockDeleteProductUseCase.Object,
            _mockFindProductsByCategoryUseCase.Object,
            _mockCreateProductUseCase.Object,
            _mockUpdateProductUseCase.Object);
    }

    [Theory]
    [InlineAutoData]
    public async Task Create_Product_Success(string name, string description, ProductCategoryDto category, decimal price,
        IReadOnlyList<string> images)
    {
        // Arrange
        var expectedProduct = new Product(name, description, (ProductCategory)category, price, images);
        expectedProduct.Create();

        _mockCreateProductUseCase.Setup(s => s.Execute(It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<ProductCategory>(),
                It.IsAny<decimal>(),
                It.IsAny<string[]>()))
            .ReturnsAsync(expectedProduct);

        // Act
        var product = await _target.CreateAsync(name, description, (ProductCategory)category, price, images);

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
        }

        _mockCreateProductUseCase.VerifyAll();
    }

    [Fact]
    public async Task Delete_Product_NotFound()
    {
        // Arrange
        _mockDeleteProductUseCase.Setup(s => s.Execute(It.IsAny<Guid>()))
            .ReturnsAsync(false);

        // Act
        var deleted = await _target.DeleteAsync(Guid.NewGuid());

        // Assert
        deleted.Should().BeFalse();
        _mockDeleteProductUseCase.VerifyAll();
    }

    [Fact]
    public async Task Delete_Product_Success()
    {
        // Arrange
        _mockDeleteProductUseCase.Setup(s => s.Execute(It.IsAny<Guid>()))
            .ReturnsAsync(true);

        // Act
        var deleted = await _target.DeleteAsync(Guid.NewGuid());

        // Assert
        deleted.Should().BeTrue();
        _mockDeleteProductUseCase.VerifyAll();
    }

    [Fact]
    public async Task GetAll_Success()
    {
        // Arrange
        var expectedProducts = new Fixture().CreateMany<Product>().ToList();
        _mockGetAllProductsUseCase.Setup(s => s.Execute())
            .ReturnsAsync(expectedProducts.ToList().AsReadOnly());

        // Act
        var products = await _target.GetAll();

        // Assert
        using (new AssertionScope())
        {
            products.Should().NotBeNull();
            products.Should().BeEquivalentTo(expectedProducts.FromEntityToDto());
            _mockGetAllProductsUseCase.VerifyAll();
        }
    }

    [Fact]
    public async Task FindByCategory_NotFound()
    {
        // Arrange
        _mockFindProductsByCategoryUseCase.Setup(s => s.Execute(It.IsAny<ProductCategory>()))
            .ReturnsAsync(Array.Empty<Product>().AsReadOnly);

        // Act
        var products = await _target.FindByCategory(ProductCategory.Drink);

        // Assert
        using (new AssertionScope())
        {
            products.Should().NotBeNull();
            products.Should().BeEmpty();
            _mockFindProductsByCategoryUseCase.VerifyAll();
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

        _mockFindProductsByCategoryUseCase.Setup(s => s.Execute(It.Is<ProductCategory>(c => c == category)))
            .ReturnsAsync(expectedProducts.AsReadOnly);

        // Act
        var products = await _target.FindByCategory(category);

        // Assert
        using (new AssertionScope())
        {
            products.Should().BeEquivalentTo(expectedProducts.FromEntityToDto());
            _mockFindProductsByCategoryUseCase.VerifyAll();
        }
    }

    [Fact]
    public async Task Update_NotFound()
    {
        // Arrange
        var fixture = new Fixture();
        fixture.Customizations.Add(new ProductGenerator());
        var product = fixture.Create<Product>();

        _mockUpdateProductUseCase.Setup(s => s.Execute(
                It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ProductCategory>(),
                It.IsAny<decimal>(), It.IsAny<IReadOnlyList<string>>()))
            .ReturnsAsync(false)
            .Verifiable();

        // Act
        var updated = await _target.UpdateAsync(product.Id, product.Name, product.Description,
            ProductCategory.Drink, product.Price, product.Images);

        // Assert
        using (new AssertionScope())
        {
            updated.Should().BeFalse();
            _mockUpdateProductUseCase.VerifyAll();
        }
    }

    [Fact]
    public async Task Update_Success()
    {
        // Arrange
        var fixture = new Fixture();
        fixture.Customizations.Add(new ProductGenerator());
        var product = fixture.Create<Product>();

        _mockUpdateProductUseCase.Setup(s => s.Execute(
                It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<ProductCategory>(),
                It.IsAny<decimal>(), It.IsAny<IReadOnlyList<string>>()))
            .ReturnsAsync(true)
            .Verifiable();

        // Act
        var updated = await _target.UpdateAsync(product.Id, product.Name, product.Description,
            ProductCategory.Drink, product.Price, product.Images);

        // Assert
        using (new AssertionScope())
        {
            updated.Should().BeTrue();
            _mockUpdateProductUseCase.VerifyAll();
        }
    }
}
