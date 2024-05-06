using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Products;

public interface ICreateProductUseCase
{
    Task<Product> Execute(string name, string description, ProductCategory category, decimal price,
        string[] images);
}