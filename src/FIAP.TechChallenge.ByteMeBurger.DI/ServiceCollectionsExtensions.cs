using System.Diagnostics.CodeAnalysis;
using FIAP.TechChallenge.ByteMeBurger.Application.DomainServices;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Customers;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Payment;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Products;
using FIAP.TechChallenge.ByteMeBurger.Controllers;
using FIAP.TechChallenge.ByteMeBurger.Controllers.Contracts;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.MercadoPago.Gateway;
using FIAP.TechChallenge.ByteMeBurger.MercadoPago.Gateway.Configuration;
using FIAP.TechChallenge.ByteMeBurger.Persistence;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FIAP.TechChallenge.ByteMeBurger.DI;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionsExtensions
{
    public static void ConfigureApplicationDI(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        ConfigHybridCache(serviceCollection, configuration);
        AddCustomerUseCases(serviceCollection);
        AddProductUseCases(serviceCollection);
        AddOrderUseCases(serviceCollection);
        AddPaymentUseCases(serviceCollection);
        RegisterControllers(serviceCollection);

        serviceCollection.AddOptionsWithValidateOnStart<MercadoPagoOptions>()
            .Bind(configuration.GetSection(MercadoPagoOptions.MercadoPago))
            .ValidateDataAnnotations();

        serviceCollection.ConfigurePersistenceApp(configuration);
        serviceCollection.ConfigureMercadoPagoGatewayApp(configuration);
    }

    private static void AddCustomerUseCases(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IFindCustomerByCpfUseCase, FindCustomerByCpfUseCase>()
            .AddScoped<ICreateCustomerUseCase, CreateCustomerUseCase>();
    }

    private static void AddOrderUseCases(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ICreateOrderUseCase, CreateOrderUseCase>()
            .AddScoped<IOrderGetAllUseCase, OrderGetAllUseCase>()
            .AddScoped<IGetOrderDetailsUseCase, GetOrderDetailsUseCase>()
            .AddScoped<IUpdateOrderStatusUseCase, UpdateOrderStatusUseCase>();
    }

    private static void AddProductUseCases(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ICreateProductUseCase, CreateProductUseCase>()
            .AddScoped<IFindProductsByCategoryUseCase, FindProductsByCategoryUseCase>()
            .AddScoped<IUpdateProductUseCase, UpdateProductUseCase>()
            .AddScoped<IGetAllProductsUseCase, GetAllProductsUseCase>()
            .AddScoped<IDeleteProductUseCase, DeleteProductUseCase>();
    }

    private static void AddPaymentUseCases(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ICreatePaymentUseCase, CreatePaymentUseCase>()
            .AddScoped<IUpdatePaymentStatusUseCase, UpdatePaymentStatusUseCase>()
            .AddScoped<IUpdateOrderPaymentUseCase, UpdateOrderPaymentUseCase>();
    }

    private static void RegisterControllers(IServiceCollection services)
    {
        services.AddScoped<ICustomerService, CustomerService>()
            .AddScoped<IProductService, ProductService>()
            .AddScoped<IOrderService, OrderService>()
            .AddScoped<IOrderTrackingCodeService, OrderTrackingCodeService>()
            .AddScoped<IPaymentService, PaymentService>();
    }

    private static void ConfigHybridCache(IServiceCollection services, IConfiguration configuration)
    {
        var hybridCacheSettings = configuration.GetSection("HybridCache")
            .Get<HybridCacheEntryOptions>();
        services.AddHybridCache(options =>
            options.DefaultEntryOptions = new HybridCacheEntryOptions()
            {
                Expiration = hybridCacheSettings.Expiration,
                LocalCacheExpiration = hybridCacheSettings.LocalCacheExpiration,
                Flags = hybridCacheSettings.Flags,
            }
        );
    }
}
