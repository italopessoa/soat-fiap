using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using Dapper;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Outgoing;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
using FIAP.TechChallenge.ByteMeBurger.Infrastructure.Dto;
using Microsoft.Extensions.Logging;

namespace FIAP.TechChallenge.ByteMeBurger.Infrastructure.Repository;

public class ProductRepositoryDapper(IDbConnection dbConnection, ILogger<ProductRepositoryDapper> logger)
    : IProductRepository
{
    public async Task<Product?> FindByIdAsync(Guid id)
    {
        logger.LogInformation("Finding product with ID: {ProductId}", id);
        var productDto = await dbConnection.QuerySingleOrDefaultAsync<ProductDto>(
            "SELECT * FROM Products WHERE Id=@Id",
            param: new { Id = id });

        if (productDto == null)
        {
            logger.LogWarning("Product with ID: {ProductId} not found", id);
        }
        else
        {
            logger.LogInformation("Product with ID: {ProductId} found", id);
        }

        return productDto;
    }

    public async Task<Product> CreateAsync(Product product)
    {
        logger.LogInformation("Creating product with name: {ProductName}", product.Name);
        var param = (ProductDto)product;
        var affectedRows = await dbConnection.ExecuteAsync(
            "insert into Products (Id, Name, Description, Category, Price, Images) values (@Id, @Name, @Description, @Category, @Price, @Images);",
            param);

        if (affectedRows > 0)
        {
            logger.LogInformation("Product with name: {ProductName} created", product.Name);
        }
        else
        {
            logger.LogError("Failed to create product with name: {ProductName}", product.Name);
        }

        return product;
    }

    public async Task<bool> DeleteAsync(Guid productId)
    {
        logger.LogInformation("Deleting product with ID: {ProductId}", productId);
        var affectedRows = await dbConnection.ExecuteAsync("delete from Products where Id = @Id;",
            new { Id = productId });

        if (affectedRows == 1)
        {
            logger.LogInformation("Product with ID: {ProductId} deleted", productId);
        }
        else
        {
            logger.LogWarning("Product with ID: {ProductId} not found", productId);
        }

        return affectedRows == 1;
    }

    public async Task<ReadOnlyCollection<Product>> GetAll()
    {
        logger.LogInformation("Getting all products");
        var productDtoList = await dbConnection.QueryAsync<ProductDto>(
            "SELECT * FROM Products");

        logger.LogInformation("Retrieved {Count} products", productDtoList.Count());
        return productDtoList.Select(p => (Product)p).ToList().AsReadOnly();
    }

    [ExcludeFromCodeCoverage(Justification =
        "unit test is not working due to moq.dapper limitations, maybe one day...")]
    public async Task<ReadOnlyCollection<Product>> FindByCategory(ProductCategory category)
    {
        logger.LogInformation("Finding products by category: {ProductCategory}", category);
        var productDtoList = await dbConnection.QueryAsync<ProductDto>(
            "SELECT * FROM Products WHERE Category = @Category",
            param: new { Category = (int)category });

        logger.LogInformation("Retrieved {Count} products", productDtoList.Count());
        return productDtoList.Select(p => (Product)p).ToList().AsReadOnly();
    }

    public async Task<bool> UpdateAsync(Product product)
    {
        logger.LogInformation("Updating product with ID: {ProductId}", product.Id);
        var affectedRows = await dbConnection.ExecuteAsync(
            "UPDATE Products SET Name=@Name, Description=@Description, Category=@Category, Price=@Price, Images=@Images WHERE Id = @Id",
            (ProductDto)product);

        if (affectedRows == 1)
        {
            logger.LogInformation("Product with ID: {ProductId} updated", product.Id);
        }
        else
        {
            logger.LogWarning("Failed to update product with ID: {ProductId}", product.Id);
        }

        return affectedRows == 1;
    }
}