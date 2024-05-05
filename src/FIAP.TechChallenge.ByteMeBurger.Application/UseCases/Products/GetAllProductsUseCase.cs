using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Outgoing;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Products;

public class GetAllProductsUseCase(IProductRepository repository) : IGetAllProductsUseCase
{
    public async Task<IReadOnlyCollection<Product>> Execute()
    {
        return await repository.GetAll() ?? Enumerable.Empty<Product>().ToList().AsReadOnly();
    }
}