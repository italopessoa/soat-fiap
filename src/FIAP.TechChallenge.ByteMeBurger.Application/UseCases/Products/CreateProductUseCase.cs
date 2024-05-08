using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Events;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Outgoing;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Products;

public class CreateProductUseCase(IProductRepository repository) : ICreateProductUseCase
{
    public async Task<Product> Execute(string name, string description, ProductCategory category, decimal price,
        string[] images)
    {
        var product = new Product(name, description, category, price, images);
        product.Create();

        var newProduct = await repository.CreateAsync(product);
        DomainEventTrigger.RaiseProductCreated(new ProductCreated(product));
        return newProduct;
    }
}
