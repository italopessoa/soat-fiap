using FIAP.TechChallenge.ByteMeBurger.Application;
using FIAP.TechChallenge.ByteMeBurger.Application.Services;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Ingoing;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Outgoing;
using FIAP.TechChallenge.ByteMeBurger.Infrastructure.Repository;

namespace FIAP.TechChallenge.ByteMeBurger.Api;

internal static class ServiceCollectionExtensions
{
    public static void RegisterFacade(this IServiceCollection services)
    {
        RegisterRepositories(services);
        RegisterServices(services);
        services.AddUseCases();
    }

    private static void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<ICustomerService, CustomerService>()
            .AddScoped<IProductService, ProductService>()
            .AddScoped<IOrderService, OrderService>();
    }

    private static void RegisterRepositories(IServiceCollection services)
    {
        services.AddScoped<IOrderRepository, OrderRepositoryDapper>()
            .AddScoped<ICustomerRepository, CustomerRepositoryDapper>()
            .AddScoped<IProductRepository, ProductRepositoryDapper>();
    }
}