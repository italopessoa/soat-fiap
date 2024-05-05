using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Outgoing;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Products;

public class FindProductsByCategoryUseCase(IProductRepository repository) : IFindProductsByCategoryUseCase
{
    public async Task<IReadOnlyCollection<Product>> Execute(ProductCategory category)
    {
        return await repository.FindByCategory(category)
               ?? Enumerable.Empty<Product>().ToList().AsReadOnly();
    }
}