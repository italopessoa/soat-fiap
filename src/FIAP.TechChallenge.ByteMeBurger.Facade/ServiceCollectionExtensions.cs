using FIAP.TechChallenge.ByteMeBurger.Application;
using FIAP.TechChallenge.ByteMeBurger.Application.Services;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Ingoing;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Outgoing;
using FIAP.TechChallenge.ByteMeBurger.Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace FIAP.TechChallenge.ByteMeBurger.Facade;

public static class ServiceCollectionExtensions
{
    public static void RegisterFacade(this IServiceCollection services)
    {
        RegisterRepositories(services);
        RegisterServices(services);
        services.AddUseCases();
    }
    
    private static void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IOrderService, OrderService>();
    }
    
    private static void RegisterRepositories(IServiceCollection services)
    {
        services.AddScoped<IOrderRepository, OrderRepositoryDapper>();
        services.AddScoped<ICustomerRepository, CustomerRepositoryDapper>();
        services.AddScoped<IProductRepository, ProductRepositoryDapper>();
    }
}