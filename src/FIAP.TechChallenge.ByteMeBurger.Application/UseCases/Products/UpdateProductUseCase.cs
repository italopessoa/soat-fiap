using Bmb.Domain.Core.Entities;
using Bmb.Domain.Core.Events;
using Bmb.Domain.Core.Interfaces;
using Bmb.Domain.Core.ValueObjects;

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
        if (await repository.UpdateAsync(currentProduct))
        {
            DomainEventTrigger.RaiseProductUpdated(new ProductUpdated((currentProduct, currentProduct)));
            return true;
        }

        return false;
    }
}
