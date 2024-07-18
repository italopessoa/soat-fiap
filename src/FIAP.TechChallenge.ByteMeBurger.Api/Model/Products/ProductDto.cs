using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Model.Products;

public class ProductDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public ProductCategory Category { get; set; }

    public decimal Price { get; set; }

    public string[] Images { get; set; } = Array.Empty<string>();

}
