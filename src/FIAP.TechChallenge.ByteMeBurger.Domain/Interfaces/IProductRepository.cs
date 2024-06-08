// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using System.Collections.ObjectModel;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;

public interface IProductRepository
{
    Task<Product?> FindByIdAsync(Guid id);

    Task<Product> CreateAsync(Product product);

    Task<bool> DeleteAsync(Guid productId);

    Task<ReadOnlyCollection<Product>> GetAll();

    Task<ReadOnlyCollection<Product>> FindByCategory(ProductCategory category);

    Task<bool> UpdateAsync(Product product);
}
