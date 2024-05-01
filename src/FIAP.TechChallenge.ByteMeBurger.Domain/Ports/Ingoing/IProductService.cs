using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Ingoing;

public interface IProductService
{
    Task<Product> CreateAsync(string name, string description, ProductCategory category, decimal price,
        IReadOnlyList<string> images);
    
    // TODO add new method
    // Task<Product?> GetAsync(Guid id);
    
    Task<bool> DeleteAsync(Guid productId);

    Task<IReadOnlyCollection<Product>> GetAll();
    
    Task<IReadOnlyCollection<Product>> FindByCategory(ProductCategory category);

    // TODO fix it, return Product instead of boolean
    Task<bool> UpdateAsync(Guid id, string name, string description, ProductCategory category, decimal price,
        IReadOnlyList<string> images);
}