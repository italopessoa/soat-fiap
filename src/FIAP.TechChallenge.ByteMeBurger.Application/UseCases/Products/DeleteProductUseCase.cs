using FIAP.TechChallenge.ByteMeBurger.Domain.Events;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Products;

public class DeleteProductUseCase(IProductRepository repository) : IDeleteProductUseCase
{
    public async Task<bool> Execute(Guid productId)
    {
        var deleted = await repository.DeleteAsync(productId);
        if (deleted)
            DomainEventTrigger.RaiseProductDeleted(productId);

        return deleted;
    }
}
