// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Ingoing;

public interface IProductService
{
    /// <summary>
    /// Create new product
    /// </summary>
    /// <param name="name">Product name</param>
    /// <param name="description">Product description</param>
    /// <param name="category">Product category <see cref="ProductCategory"/></param>
    /// <param name="price">Product price</param>
    /// <param name="images">Product images</param>
    /// <returns>Product entity.</returns>
    Task<Product> CreateAsync(string name, string description, ProductCategory category, decimal price,
        IReadOnlyList<string> images);

    // TODO add new method use issue https://github.com/soat-fiap/FIAP.TechChallenge.ByteMeBurger/issues/32
    // Task<Product?> GetAsync(Guid id);

    /// <summary>
    /// Delete product
    /// </summary>
    /// <param name="productId">Product Id</param>
    /// <returns>Is deleted</returns>
    Task<bool> DeleteAsync(Guid productId);

    /// <summary>
    /// Get all products
    /// </summary>
    /// <returns>List of products</returns>
    Task<IReadOnlyCollection<Product>> GetAll();

    /// <summary>
    /// Find products by category
    /// </summary>
    /// <param name="category">Product category <see cref="ProductCategory"/></param>
    /// <returns>Product list</returns>
    Task<IReadOnlyCollection<Product>> FindByCategory(ProductCategory category);

    // TODO fix it, return Product instead of boolean
    /// <summary>
    /// Update product
    /// </summary>
    /// <param name="id">Product Id.</param>
    /// <param name="name">Product name.</param>
    /// <param name="description">Product description.</param>
    /// <param name="category">Product category.</param>
    /// <param name="price">Product price.</param>
    /// <param name="images">Product images.</param>
    /// <returns>Is updated</returns>
    Task<bool> UpdateAsync(Guid id, string name, string description, ProductCategory category, decimal price,
        IReadOnlyList<string> images);
}
