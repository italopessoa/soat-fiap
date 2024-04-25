using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Ingoing;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Outgoing;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.Services;

public class ProductService(IProductRepository repository) : IProductService
{
    private readonly IProductRepository _repository = repository;

    public async Task<Product> CreateAsync(string name, string description, ProductCategory category, decimal price,
        IReadOnlyList<string> images)
    {
        var newProduct = new Product(name, description, category, price, images);
        newProduct.Create();
        return await _repository.CreateAsync(newProduct);
    }

    public async Task<bool> DeleteAsync(Guid productId)
    {
        return await _repository.DeleteAsync(productId);
    }

    public async Task<IReadOnlyCollection<Product>> GetAll()
    {
        return (await _repository.GetAll()) ?? Enumerable.Empty<Product>().ToList().AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Product>> FindByCategory(ProductCategory category)
    {
        return (await _repository.FindByCategory(category))
               ?? Enumerable.Empty<Product>().ToList().AsReadOnly();
    }

    public async Task<bool> UpdateAsync(Guid id, string name, string description, ProductCategory category,
        decimal price,
        IReadOnlyList<string> images)
    {
        var currentProduct = await _repository.FindByIdAsync(id);
        if (currentProduct is null)
        {
            return false;
        }

        currentProduct.Update(new Product(id, name, description, category, price, images));
        return await _repository.UpdateAsync(currentProduct);
    }
}