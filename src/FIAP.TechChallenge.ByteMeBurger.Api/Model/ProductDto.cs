using System.ComponentModel.DataAnnotations;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Model;

public class ProductDto
{
    public Guid Id { get; set; }

    [Required] 
    public string Name { get; set; }

    [Required] 
    public string Description { get; set; }
    
    public ProductCategory Category { get; set; }

    public decimal Price { get; set; }

    public string[] Images { get; set; }

    public ProductDto()
    {
        
    }
    
    public ProductDto(Product product)
    {
        Id = product.Id;
        Name = product.Name;
        Description = product.Description;
        Category = product.Category;
        Price = product.Price;
        Images = product.Images.ToArray();
    }
}