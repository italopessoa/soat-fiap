using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Products;

public interface IUpdateProductUseCase
{
    Task<bool> Execute(Guid id, string name, string description, ProductCategory category, decimal price,
        IReadOnlyList<string> images);
}