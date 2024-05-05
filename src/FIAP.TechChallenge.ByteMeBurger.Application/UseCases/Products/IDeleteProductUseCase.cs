namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Products;

public interface IDeleteProductUseCase
{
    Task<bool> Execute(Guid productId);
}