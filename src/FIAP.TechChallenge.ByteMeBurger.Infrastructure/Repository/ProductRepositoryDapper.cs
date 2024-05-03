using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using Dapper;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Outgoing;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
using FIAP.TechChallenge.ByteMeBurger.Infrastructure.Dto;

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
        var productDto = await _dbConnection.QuerySingleOrDefaultAsync<ProductDto>(
            "SELECT * FROM Products WHERE Id=@Id",
            param: new { Id = id });

        return productDto;
    }

    public async Task<Product> CreateAsync(Product product)
    {
        var param = (ProductDto)product;
        var affectedRows = await _dbConnection.ExecuteAsync(
            "insert into Products (Id, Name, Description, Category, Price, Images) values (@Id, @Name, @Description, @Category, @Price, @Images);",
            param);
        return product;
    }

    public async Task<bool> DeleteAsync(Guid productId)
    {
        var affectedRows = await _dbConnection.ExecuteAsync("delete from Products where Id = @Id;",
            new { Id = productId });
        return affectedRows == 1;
    }

    public async Task<ReadOnlyCollection<Product>> GetAll()
    {
        var productDtoList = await _dbConnection.QueryAsync<ProductDto>(
            "SELECT * FROM Products");

        return productDtoList.Select(p => (Product)p).ToList().AsReadOnly();
    }

    [ExcludeFromCodeCoverage(Justification =
        "unit test is not working due to moq.dapper limitations, maybe one day...")]
    public async Task<ReadOnlyCollection<Product>> FindByCategory(ProductCategory category)
    {
        var productDtoList = await _dbConnection.QueryAsync<ProductDto>(
            "SELECT * FROM Products WHERE Category = @Category",
            param: new { Category = (int)category });

        return productDtoList.Select(p => (Product)p).ToList().AsReadOnly();
    }

    public async Task<bool> UpdateAsync(Product product)
    {
        var affectedRows = await _dbConnection.ExecuteAsync(
            "UPDATE Products SET Name=@Name, Description=@Description, Category=@Category, Price=@Price, Images=@Images WHERE Id = @Id",
            (ProductDto)product);

        return affectedRows == 1;
    }
}