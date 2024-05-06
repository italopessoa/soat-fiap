using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Outgoing;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Products;

public class CreateProductUseCase(IProductRepository repository) : ICreateProductUseCase
{
    public async Task<Product> Execute(string name, string description, ProductCategory category, decimal price,
        string[] images)
    {
        var newProduct = new Product(name, description, category, price, images);
        newProduct.Create();
        return await repository.CreateAsync(newProduct);
    }
}