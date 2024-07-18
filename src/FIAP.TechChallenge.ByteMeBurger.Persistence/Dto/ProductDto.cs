using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Persistence.Dto;

internal class ProductDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int Category { get; set; }

    public decimal Price { get; set; }

    public string Images { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime? LastUpdate { get; set; }

    public ProductDto()
    {
    }

    public static implicit operator Product(ProductDto? dto) => ToDomain(dto);


    public static explicit operator ProductDto(Product product) => new()
    {
        Id = product.Id,
        Name = product.Name,
        Description = product.Description,
        Category = (int)product.Category,
        Price = product.Price,
        CreationDate = product.CreationDate,
        LastUpdate = product.LastUpdate,
        Images = string.Join("|", product.Images)
    };

    private static Product? ToDomain(ProductDto? dto)
    {
        if (dto is null) return null;

        var images = dto.Images?.Split("|") ?? Array.Empty<string>();
        return new Product(dto.Id, dto.Name, dto.Description, (ProductCategory)dto.Category, dto.Price, images);
    }
}
