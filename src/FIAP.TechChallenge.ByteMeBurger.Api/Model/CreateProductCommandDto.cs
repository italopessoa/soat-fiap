using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
using Microsoft.Build.Framework;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Model;

public class CreateProductCommandDto
{
    [Required] public string Name { get; set; }

    [Required] public string Description { get; set; }

    [Required] public ProductCategory Category { get; set; }

    [Required] public decimal Price { get; set; }

    public string[] Images { get; set; }

    // TODO make this iternal
    public Product ToProduct() => new Product(Guid.NewGuid(), Name, Description, Category, Price, Images);
}