using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Persistence.Repository;

[ExcludeFromCodeCoverage]
public class InMemoryProductRepository(IReadOnlyList<Product> products) : IProductRepository
{
    private readonly List<Product> _products = products.ToList();

    public Task<Product?> FindByIdAsync(Guid id)
    {
        var product = _products.AsReadOnly().FirstOrDefault(p => p.Id == id);

        if (product is not null)
            return Task.FromResult(new Product(product.Id, product.Name, product.Description, product.Category,
                product.Price, product.Images))!;

        return Task.FromResult(product);
    }

    public Task<Product> CreateAsync(Product product)
    {
        _products.Add(product);
        return Task.FromResult(_products.ElementAt(_products.Count - 1));
    }

    public Task<bool> DeleteAsync(Guid productId)
    {
        var deleted = false;
        var productIndex = _products.FindIndex(p => p.Id == productId);
        if (productIndex > 0)
        {
            _products.RemoveAt(productIndex);
            deleted = true;
        }

        return Task.FromResult(deleted);
    }

    public Task<ReadOnlyCollection<Product>> GetAll()
    {
        return Task.FromResult(_products.AsReadOnly());
    }

    public Task<ReadOnlyCollection<Product>> FindByCategory(ProductCategory category)
    {
        return Task.FromResult(_products
            .Where(p => p.Category == category)
            .ToList()
            .AsReadOnly());
    }

    public Task<bool> UpdateAsync(Product product)
    {
        var productIndex = _products.FindIndex(p => p.Id == product.Id);
        if (productIndex < 0)
            return Task.FromResult(false);

        _products[productIndex] = product;

        return Task.FromResult(true);
    }

    public Task<bool> UpdateAsync(Guid id, string name, string description, ProductCategory category, decimal price,
        string[] images)
    {
        var updated = false;
        var productIndex = _products.FindIndex(p => p.Id == id);
        if (productIndex >= 0)
        {
            _products[productIndex].Update(new Product(name, description, category, price, images));
            updated = true;
        }

        return Task.FromResult(updated);
    }
}
