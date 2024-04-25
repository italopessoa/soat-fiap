using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Ingoing;

public interface IProductService
{
    Task<Product> CreateAsync(string name, string description, ProductCategory category, decimal price,
        IReadOnlyList<string> images);
    
    Task<bool> DeleteAsync(Guid productId);

    Task<IReadOnlyCollection<Product>> GetAll();
    
    Task<IReadOnlyCollection<Product>> FindByCategory(ProductCategory category);

    Task<bool> UpdateAsync(Guid id, string name, string description, ProductCategory category, decimal price,
        IReadOnlyList<string> images);
}