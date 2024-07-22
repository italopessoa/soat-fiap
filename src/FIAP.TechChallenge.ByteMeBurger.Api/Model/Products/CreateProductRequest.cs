using FIAP.TechChallenge.ByteMeBurger.Controllers.Dto;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
using Microsoft.Build.Framework;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Model.Products;

public class CreateProductRequest
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public ProductCategoryDto Category { get; set; }

    [Required]
    public decimal Price { get; set; }

    public string[] Images { get; set; } = Array.Empty<string>();

    // TODO make this iternal
    public Product ToProduct() =>
        new Product(Guid.NewGuid(), Name, Description, (ProductCategory)Category, Price, Images);
}
