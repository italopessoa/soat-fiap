using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Products;

public interface IFindProductsByCategoryUseCase
{
    Task<IReadOnlyCollection<Product>> Execute(ProductCategory category);
}