using Bmb.Domain.Core.Entities;
using Bmb.Domain.Core.Events;
using Bmb.Domain.Core.Interfaces;
using Bmb.Domain.Core.ValueObjects;
using ProductCreated = Bmb.Domain.Core.Events.Integration.ProductCreated;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Products;

public class CreateProductUseCase(IProductRepository repository, IDispatcher dispatcher) : ICreateProductUseCase
{
    public async Task<Product> Execute(string name, string description, ProductCategory category, decimal price,
        string[] images)
    {
        var product = new Product(name, description, category, price, images);
        product.Create();

        var newProduct = await repository.CreateAsync(product);
        await dispatcher.PublishIntegrationAsync(MapToEvent(product), default);
        return newProduct;
    }

    private static ProductCreated MapToEvent(Product product)
    {
        return new ProductCreated(product.Id, product.Name,
            product.Category.ToString());
    }
}
