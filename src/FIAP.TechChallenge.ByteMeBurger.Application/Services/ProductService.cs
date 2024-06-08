// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Products;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.Services;

public class ProductService(
    IGetAllProductsUseCase getAllProductsUseCase,
    IDeleteProductUseCase deleteProductUseCase,
    IFindProductsByCategoryUseCase findProductsByCategoryUseCase,
    ICreateProductUseCase createProductUseCase,
    IUpdateProductUseCase updateProductUseCase) : IProductService
{
    public async Task<Product> CreateAsync(string name, string description, ProductCategory category, decimal price,
        IReadOnlyList<string> images)
    {
        return await createProductUseCase.Execute(name, description, category, price, images.ToArray());
    }

    public async Task<bool> DeleteAsync(Guid productId)
    {
        return await deleteProductUseCase.Execute(productId);
    }

    public async Task<IReadOnlyCollection<Product>> GetAll()
    {
        return await getAllProductsUseCase.Execute();
    }

    public async Task<IReadOnlyCollection<Product>> FindByCategory(ProductCategory category)
    {
        return await findProductsByCategoryUseCase.Execute(category);
    }

    public async Task<bool> UpdateAsync(Guid id, string name, string description, ProductCategory category,
        decimal price,
        IReadOnlyList<string> images)
    {
        return await updateProductUseCase.Execute(id, name, description, category, price, images);
    }
}
