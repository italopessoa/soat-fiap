using FIAP.TechChallenge.ByteMeBurger.Controllers.Dto;
using Bmb.Domain.Core.Entities;
using Bmb.Domain.Core.ValueObjects;
using Microsoft.Build.Framework;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Model.Products;

/// <summary>
/// Create product payload
/// </summary>
public class CreateProductRequest
{
    /// <summary>
    /// Product name
    /// </summary>
    [Required]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Product description
    /// </summary>
    [Required]
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Product category
    /// </summary>
    [Required]
    public ProductCategoryDto Category { get; set; }

    /// <summary>
    /// Product price
    /// </summary>
    [Required]
    public decimal Price { get; set; }

    /// <summary>
    /// Product images
    /// </summary>
    public string[] Images { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Convert to Domain
    /// </summary>
    /// <returns></returns>
    internal Product ToProduct() =>
        new Product(Guid.NewGuid(), Name, Description, (ProductCategory)Category, Price, Images);
}
