using System.Collections.ObjectModel;
using System.Data;
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

    public Task<Product?> FindByIdAsync(Guid id)
    {
        throw new NotImplementedException();
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

    public Task<ReadOnlyCollection<Product>> FindByCategory(ProductCategory category)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateAsync(Product product)
    {
        throw new NotImplementedException();
    }
}