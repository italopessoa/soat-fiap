using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Test.ValueObjects;

public class ProductTests
{
    [Fact]
    public void Product_InvalidName_ThrowsError()
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<ArgumentException>(() => new Product(string.Empty, "description", ProductCategory.Beverage, 10m,
            Enumerable.Empty<string>().ToList()));
    }

    [Fact]
    public void Product_InvalidDescription_ThrowsError()
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<ArgumentException>(() =>
            new Product("product name", string.Empty, ProductCategory.Beverage, 10m,
                Enumerable.Empty<string>().ToList()));
    }

    [Fact]
    public void Product_InvalidPrice_ThrowsError()
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new Product("product name", "description", ProductCategory.Beverage, -1m,
                Enumerable.Empty<string>().ToList()));
    }

    [Theory]
    [InlineData("product name", "description", ProductCategory.Beverage, 1)]
    public void Product_ValidProduct(string name, string description, ProductCategory category, decimal price)
    {
        // Arrange
        // Act
        var product = new Product(name, description, category, price, Enumerable.Empty<string>().ToList());
       
        // Assert
        Assert.Equal(category, product.Category);
        Assert.Equal(description.ToUpper(),product.Description);
        Assert.Equal(name.ToUpper(),product.Name);
        Assert.Equal(price,product.Price);
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
        Assert.True(productA.Equals(productB));
        Assert.Equal(productName.ToUpper(), productA.Name);
        Assert.Equal(productDescription.ToUpper(), productB.Description);
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
        Assert.False(productA.Equals(productB));
    }
}