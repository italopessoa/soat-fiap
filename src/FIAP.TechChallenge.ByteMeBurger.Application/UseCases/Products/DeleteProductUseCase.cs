using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Outgoing;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Products;

public class DeleteProductUseCase(IProductRepository repository) : IDeleteProductUseCase
{
    public async Task<bool> Execute(Guid productId)
    {
        return await repository.DeleteAsync(productId);
    }
}