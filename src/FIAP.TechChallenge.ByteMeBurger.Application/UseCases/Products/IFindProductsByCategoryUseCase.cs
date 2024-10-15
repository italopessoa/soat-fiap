using Bmb.Domain.Core.Entities;
using Bmb.Domain.Core.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Products;

public interface IFindProductsByCategoryUseCase
{
    Task<IReadOnlyCollection<Product>> Execute(ProductCategory category);
}
