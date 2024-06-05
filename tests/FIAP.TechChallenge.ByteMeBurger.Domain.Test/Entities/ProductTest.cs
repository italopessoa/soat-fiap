// Copyright (c) 2024, Italo Pessoa (https://github.com/italopessoa)
// All rights reserved.
//
// This source code is licensed under the BSD-style license found in the
// LICENSE file in the root directory of this source tree.

using AutoFixture.Xunit2;
using FIAP.TechChallenge.ByteMeBurger.Domain.Base;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
using FluentAssertions;
using FluentAssertions.Execution;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Test.Entities;

public class ProductTest
{
    [Fact]
    public void NewProduct_InvalidName()
    {
        // Arrange
        // Act
        var product = () => new Product(string.Empty, "description", ProductCategory.Drink, 10m, []);

        // Assert
        using (new AssertionScope())
        {
            product.Should().ThrowExactly<DomainException>();
        }
    }

    [Fact]
    public void NewProduct_InvalidDescription()
    {
        // Arrange
        // Act
        var product = () => new Product("name", string.Empty, ProductCategory.Drink, 10m, []);

        // Assert
        using (new AssertionScope())
        {
            product.Should().ThrowExactly<DomainException>();
        }
    }

    [Fact]
    public void NewProduct_InvalidPrice()
    {
        // Arrange
        // Act
        var product = () => new Product("name", "description", ProductCategory.Drink, 0, []);

        // Assert
        using (new AssertionScope())
        {
            product.Should().ThrowExactly<DomainException>();
        }
    }

    [Theory]
    [InlineAutoData]
    public void NewProduct_Valid(string name, string description, ProductCategory category, decimal price,
        string[] images)
    {
        // Arrange
        // Act
        var product = new Product(name, description, category, price, images);

        // Assert
        using (new AssertionScope())
        {
            product.Id.Should().NotBe(Guid.Empty);
            product.Name.Should().Be(name.ToUpper());
            product.Description.Should().Be(description.ToUpper());
            product.Category.Should().Be(category);
            product.Images.Should().BeEquivalentTo(images);
        }
    }

    [Theory]
    [InlineAutoData]
    public void ExistentProduct_Valid(Guid id, string name, string description, ProductCategory category, decimal price,
        string[] images)
    {
        // Arrange
        // Act
        var product = new Product(id, name, description, category, price, images);

        // Assert
        using (new AssertionScope())
        {
            product.Id.Should().Be(id);
            product.Name.Should().Be(name.ToUpper());
            product.Description.Should().Be(description.ToUpper());
            product.Category.Should().Be(category);
            product.Images.Should().BeEquivalentTo(images);
        }
    }
}
