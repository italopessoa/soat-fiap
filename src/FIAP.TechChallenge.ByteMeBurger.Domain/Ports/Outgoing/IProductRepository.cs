using System.Collections.ObjectModel;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Outgoing;

public interface IProductRepository
{
    Task<Product?> FindByIdAsync(Guid id);
    
    Task<Product> CreateAsync(Product product);
    
    Task<bool> DeleteAsync(Guid productId);

    Task<ReadOnlyCollection<Product>> GetAll();
    
    Task<ReadOnlyCollection<Product>> FindByCategory(ProductCategory category);

    Task<bool> UpdateAsync(Product product);
}