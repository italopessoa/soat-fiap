// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Products;

public class FindProductsByCategoryUseCase(IProductRepository repository) : IFindProductsByCategoryUseCase
{
    public async Task<IReadOnlyCollection<Product>> Execute(ProductCategory category)
    {
        return await repository.FindByCategory(category)
               ?? Array.Empty<Product>().ToList().AsReadOnly();
    }
}
