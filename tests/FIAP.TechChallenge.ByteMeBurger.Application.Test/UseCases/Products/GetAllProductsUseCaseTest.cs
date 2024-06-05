// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using System.Collections.ObjectModel;
using AutoFixture;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Products;

namespace FIAP.TechChallenge.ByteMeBurger.Application.Test.UseCases.Products;

[TestSubject(typeof(GetAllProductsUseCase))]
public class GetAllProductsUseCaseTest : BaseProductsUseCaseTests
{
    [Fact]
    public async Task GetAll_Empty()
    {
        // Arrange
        _productRepository.Setup(s => s.GetAll())
            .ReturnsAsync((ReadOnlyCollection<Product>)default!);

        // Act
        var products = await _getAllProductsUseCase.Execute();

        // Assert
        using (new AssertionScope())
        {
            products.Should().NotBeNull();
            products.Should().BeEmpty();
            _productRepository.VerifyAll();
        }
    }

    [Fact]
    public async Task GetAll_Success()
    {
        // Arrange
        var expectedProducts = new Fixture().CreateMany<Product>().ToList();
        _productRepository.Setup(s => s.GetAll())
            .ReturnsAsync(expectedProducts.ToList().AsReadOnly());

        // Act
        var products = await _getAllProductsUseCase.Execute();

        // Assert
        using (new AssertionScope())
        {
            products.Should().NotBeNull();
            products.Should().BeEquivalentTo(expectedProducts);
            _productRepository.VerifyAll();
        }
    }
}
