using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
using FluentAssertions;
using FluentAssertions.Execution;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Test.ValueObjects;

public class ProductTests
{
    [Fact]
    public void Product_InvalidName_ThrowsError()
    {
        // Arrange
        // Act
        var func = () => new Product(string.Empty, "description", ProductCategory.Beverage, 10m,
            Enumerable.Empty<string>().ToList());
        // Assert
        func.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Product_InvalidDescription_ThrowsError()
    {
        // Arrange
        // Act
        var func = () => new Product("product name", string.Empty, ProductCategory.Beverage, 10m,
            Enumerable.Empty<string>().ToList());

        // Assert
        func.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Product_InvalidPrice_ThrowsError()
    {
        // Arrange
        // Act
        var func = () => new Product("product name", "description", ProductCategory.Beverage, -1m,
            Enumerable.Empty<string>().ToList());

        // Assert
        func.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Theory]
    [InlineData("product name", "description", ProductCategory.Beverage, 1)]
    public void Product_ValidProduct(string name, string description, ProductCategory category, decimal price)
    {
        // Arrange
        // Act
        var product = new Product(name, description, category, price, Enumerable.Empty<string>().ToList());

        // Assert
        using (new AssertionScope())
        {
            category.Should().Be(product.Category);
            description.ToUpper().Should().Be(product.Description);
            name.ToUpper().Should().Be(product.Name);
            price.Should().Be(product.Price);
        }
    }

    [Fact]
    public void Product_CompareProduct_True()
    {
        // Arrange
        // Act
        const string productName = "product name";
        const string productDescription = "description";

        var productA = new Product("product Name", productDescription, ProductCategory.Beverage, 1m,
            Enumerable.Empty<string>().ToList());
        var productB = new Product(productName, "descriptioN", ProductCategory.Beverage, 1m,
            Enumerable.Empty<string>().ToList());

        // Assert
        productA.Should().Be(productB);
    }

    [Fact]
    public void Product_CompareProduct_False()
    {
        // Arrange
        // Act
        var productA = new Product("product a name", "description", ProductCategory.Beverage, 1m,
            Enumerable.Empty<string>().ToList());
        var productB = new Product("product name", "description", ProductCategory.Beverage, 1m,
            Enumerable.Empty<string>().ToList());

        // Assert
        productA.Should().NotBe(productB);
    }
}