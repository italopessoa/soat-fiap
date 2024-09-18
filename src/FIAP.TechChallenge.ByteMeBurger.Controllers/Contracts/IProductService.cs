using FIAP.TechChallenge.ByteMeBurger.Controllers.Dto;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Controllers.Contracts;

public interface IProductService
{
    /// <summary>
    /// Create new product
    /// </summary>
    /// <param name="name">Product name</param>
    /// <param name="description">Product description</param>
    /// <param name="category">Product category <see cref="ProductCategory"/></param>
    /// <param name="price">Product price</param>
    /// <param name="images">Product images</param>
    /// <returns>Product entity.</returns>
    Task<ProductDto> CreateAsync(string name, string description, ProductCategory category, decimal price,
        IReadOnlyList<string> images);

    // TODO add new method use issue https://github.com/soat-fiap/FIAP.TechChallenge.ByteMeBurger/issues/32

    /// <summary>
    /// Delete product
    /// </summary>
    /// <param name="productId">Product Id</param>
    /// <returns>Is deleted</returns>
    Task<bool> DeleteAsync(Guid productId);

    /// <summary>
    /// Get all products
    /// </summary>
    /// <returns>List of products</returns>
    Task<IReadOnlyCollection<ProductDto>> GetAll();

    /// <summary>
    /// Find products by category
    /// </summary>
    /// <param name="category">Product category <see cref="ProductCategory"/></param>
    /// <returns>Product list</returns>
    Task<IReadOnlyCollection<ProductDto>> FindByCategory(ProductCategory category);

    // TODO fix it, return Product instead of boolean
    /// <summary>
    /// Update product
    /// </summary>
    /// <param name="id">Product Id.</param>
    /// <param name="name">Product name.</param>
    /// <param name="description">Product description.</param>
    /// <param name="category">Product category.</param>
    /// <param name="price">Product price.</param>
    /// <param name="images">Product images.</param>
    /// <returns>Is updated</returns>
    Task<bool> UpdateAsync(Guid id, string name, string description, ProductCategory category, decimal price,
        IReadOnlyList<string> images);
}
