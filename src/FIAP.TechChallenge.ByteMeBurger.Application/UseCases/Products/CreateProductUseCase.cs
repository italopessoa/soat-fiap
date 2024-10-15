using Bmb.Domain.Core.Entities;
using Bmb.Domain.Core.Events;
using Bmb.Domain.Core.Interfaces;
using Bmb.Domain.Core.ValueObjects;

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
