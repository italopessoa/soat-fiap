using Bmb.Domain.Core.Entities;
using Bmb.Domain.Core.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Products;

public interface ICreateProductUseCase
{
    Task<Product> Execute(string name, string description, ProductCategory category, decimal price,
        string[] images);
}
