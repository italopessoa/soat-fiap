using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Products;
using FIAP.TechChallenge.ByteMeBurger.Controllers.Contracts;
using FIAP.TechChallenge.ByteMeBurger.Controllers.Dto;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Controllers;

public class ProductService(
    IGetAllProductsUseCase getAllProductsUseCase,
    IDeleteProductUseCase deleteProductUseCase,
    IFindProductsByCategoryUseCase findProductsByCategoryUseCase,
    ICreateProductUseCase createProductUseCase,
    IUpdateProductUseCase updateProductUseCase) : IProductService
{
    public async Task<ProductDto> CreateAsync(string name, string description, ProductCategory category, decimal price,
        IReadOnlyList<string> images)
    {
        var product = await createProductUseCase.Execute(name, description, category, price, images.ToArray());
        return product.FromEntityToDto();
    }

    public async Task<bool> DeleteAsync(Guid productId)
    {
        return await deleteProductUseCase.Execute(productId);
    }

    public async Task<IReadOnlyCollection<ProductDto>> GetAll()
    {
        var products = await getAllProductsUseCase.Execute();
        return products.FromEntityToDto();
    }

    public async Task<IReadOnlyCollection<ProductDto>> FindByCategory(ProductCategory category)
    {
        var products = await findProductsByCategoryUseCase.Execute(category);
        return products.FromEntityToDto();
    }

    public async Task<bool> UpdateAsync(Guid id, string name, string description, ProductCategory category,
        decimal price,
        IReadOnlyList<string> images)
    {
        return await updateProductUseCase.Execute(id, name, description, category, price, images);
    }
}
