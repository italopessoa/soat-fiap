using Bmb.Domain.Core.Entities;
using Bmb.Domain.Core.Interfaces;
using Bmb.Domain.Core.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Products;

public class FindProductsByCategoryUseCase(IProductRepository repository) : IFindProductsByCategoryUseCase
{
    public async Task<IReadOnlyCollection<Product>> Execute(ProductCategory category)
    {
        return await repository.FindByCategory(category)
               ?? Array.Empty<Product>().ToList().AsReadOnly();
    }
}
