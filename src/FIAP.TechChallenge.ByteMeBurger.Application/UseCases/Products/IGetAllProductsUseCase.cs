using Bmb.Domain.Core.Entities;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Products;

public interface IGetAllProductsUseCase
{
    Task<IReadOnlyCollection<Product>> Execute();
}
