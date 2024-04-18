using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Test.ValueObjects;

public class ProductTests
{
    [Fact]
    public void Product_InvalidName_ThrowsError()
    {
        Assert.Throws<ArgumentException>(() => new Product(string.Empty, "description", ProductCategory.Beverage, 10m,
            Enumerable.Empty<string>().ToList()));
    }

    [Fact]
    public void Product_InvalidDescription_ThrowsError()
    {
        Assert.Throws<ArgumentException>(() =>
            new Product("product name", string.Empty, ProductCategory.Beverage, 10m,
                Enumerable.Empty<string>().ToList()));
    }

    [Fact]
    public void Product_InvalidPrice_ThrowsError()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new Product("product name", "description", ProductCategory.Beverage, -1m,
                Enumerable.Empty<string>().ToList()));
    }

    [Theory]
    [InlineData("product name", "description", ProductCategory.Beverage, 1)]
    public void Product_ValidProduct(string name, string description, ProductCategory category, decimal price)
    {
        var product = new Product(name, description, category, price, Enumerable.Empty<string>().ToList());
        Assert.Equal(product.Category, category);
        Assert.Equal(product.Description, description);
        Assert.Equal(product.Name, name);
        Assert.Equal(product.Price, price);
    }

    [Fact]
    public void Product_CompareProduct_True()
    {
        var productA = new Product("product name", "description", ProductCategory.Beverage, 1m,
            Enumerable.Empty<string>().ToList());
        var productB = new Product("product name", "description", ProductCategory.Beverage, 1m,
            Enumerable.Empty<string>().ToList());
        Assert.True(productA.Equals(productB));
    }

    [Fact]
    public void Product_CompareProduct_False()
    {
        var productA = new Product("product a name", "description", ProductCategory.Beverage, 1m,
            Enumerable.Empty<string>().ToList());
        var productB = new Product("product name", "description", ProductCategory.Beverage, 1m,
            Enumerable.Empty<string>().ToList());
        Assert.False(productA.Equals(productB));
    }
}