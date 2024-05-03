using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using Dapper;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Outgoing;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Infrastructure.Repository;

public class ProductRepositoryDapper : IProductRepository
{
    private readonly IDbConnection _dbConnection;

    public ProductRepositoryDapper(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<Product?> FindByIdAsync(Guid id)
    {
        var product = (await _dbConnection.QueryAsync<Product, string, Product>(
            "SELECT * FROM Products WHERE Id=@Id",
            (product, s) =>
            {
                product.SetImages(s.Split("|"));
                return product;
            },
            splitOn: "Images", param: new { Id = id })).FirstOrDefault();
        return product;
    }

    public async Task<Product> CreateAsync(Product product)
    {
        var affectedRows = await _dbConnection.ExecuteAsync(
            "insert into Products (Id, Name, Description, Category, Price, Images) values (@Id, @Name, @Description, @Category, @Price, @Images);",
            new
            {
                product.Id,
                product.Name,
                product.Description,
                product.Category,
                product.Price,
                Images = string.Join("|", product.Images)
            });
        return product;
    }

    public async Task<bool> DeleteAsync(Guid productId)
    {
        var affectedRows = await _dbConnection.ExecuteAsync("delete from Products where Id = @Id;",
            new { Id = productId });
        return affectedRows == 1;
    }

    public Task<ReadOnlyCollection<Product>> GetAll()
    {
        throw new NotImplementedException();
    }

    [ExcludeFromCodeCoverage(Justification =
        "unit test is not working due to moq.dapper limitations, maybe one day...")]
    public async Task<ReadOnlyCollection<Product>> FindByCategory(ProductCategory category)
    {
        return (await _dbConnection.QueryAsync<Product, string, Product>(
            "SELECT * FROM Products WHERE Category = @Category",
            (product, s) =>
            {
                product.SetImages(s.Split("|"));
                return product;
            },
            splitOn: "Images", param: new { Category = (int)category })).ToList().AsReadOnly();
    }

    public async Task<bool> UpdateAsync(Product product)
    {
        var affectedRows = await _dbConnection.ExecuteAsync(
            "UPDATE Products SET Name=@Name, Description=@Description, Category=@Category, Price=@Price, Images=@Images WHERE Id = @Id",
            new
            {
                product.Id,
                product.Name,
                product.Description,
                product.Category,
                product.Price,
                Images = string.Join("|", product.Images)
            });

        return affectedRows == 1;
    }
}