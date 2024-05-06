using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Outgoing;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Products;

public class UpdateProductUseCase(IProductRepository repository) : IUpdateProductUseCase
{
    public async Task<bool> Execute(Guid id, string name, string description, ProductCategory category, decimal price,
        IReadOnlyList<string> images)
    {
        var currentProduct = await repository.FindByIdAsync(id);
        if (currentProduct is null)
        {
            return false;
        }

        currentProduct.Update(new Product(id, name, description, category, price, images));
        return await repository.UpdateAsync(currentProduct);
    }
}