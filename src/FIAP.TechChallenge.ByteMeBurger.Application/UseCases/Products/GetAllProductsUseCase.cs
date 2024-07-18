using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Products;

public class GetAllProductsUseCase(IProductRepository repository) : IGetAllProductsUseCase
{
    public async Task<IReadOnlyCollection<Product>> Execute()
    {
        return await repository.GetAll() ?? Array.Empty<Product>().AsReadOnly() ;
    }
}
