using FIAP.TechChallenge.ByteMeBurger.Application.Services;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Ingoing;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Outgoing;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
using FIAP.TechChallenge.ByteMeBurger.Infrastructure.Repository;

namespace FIAP.TechChallenge.ByteMeBurger.Api;

internal static class ServiceCollectionExtensions
{
    internal static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IOrderRepository, OrderRepositoryDapper>();
        services.AddScoped<ICustomerRepository, CustomerRepositoryDapper>();
        services.AddScoped<IProductRepository, ProductRepositoryDapper>();
        // services.AddSingleton<ICustomerRepository>(new InMemoryCustomerRepository(new[]
        // {
        //     new Customer("663.781.241-24", "Pietro Thales Anderson Rodrigues", "pietro_thales_rodrigues@silicotex.net")
        // }));
        
        // services.AddSingleton<IProductRepository>(new InMemoryProductRepository(new[]
        // {
        //     new Product("pao com ovo", "pao com ovo", ProductCategory.Meal, 2.5m, []),
        //     new Product("milkshake chocrante", "milkshake tijolo do bob'as", ProductCategory.SweatsNTreats, 2.5m, []),
        //     new Product("h20", "h20", ProductCategory.Beverage, 2.5m, []),
        //     new Product("batata frita", "batata frita", ProductCategory.FriesAndSides, 2.5m, [])
        // }));
    }
}