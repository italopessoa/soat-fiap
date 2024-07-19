using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using FIAP.TechChallenge.ByteMeBurger.Application.DomainServices;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Customers;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Payment;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Products;
using FIAP.TechChallenge.ByteMeBurger.Controllers;
using FIAP.TechChallenge.ByteMeBurger.Controllers.Contracts;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
using FIAP.TechChallenge.ByteMeBurger.FakePayment.Gateway;
using FIAP.TechChallenge.ByteMeBurger.MercadoPago.Gateway;
using FIAP.TechChallenge.ByteMeBurger.MercadoPago.Gateway.Configuration;
using FIAP.TechChallenge.ByteMeBurger.MercadoPago.Gateway.Security;
using FIAP.TechChallenge.ByteMeBurger.Persistence.Repository;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;

namespace FIAP.TechChallenge.ByteMeBurger.DI;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionsExtensions
{
    public static void ConfigureApplicationDI(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        ConfigurePersistenceApp(serviceCollection, configuration);
        ConfigurePaymentGateway(serviceCollection, configuration);
        ConfigHybridCache(serviceCollection, configuration);
        AddCustomerUseCases(serviceCollection);
        AddProductUseCases(serviceCollection);
        AddOrderUseCases(serviceCollection);
        AddPaymentUseCases(serviceCollection);
        RegisterControllers(serviceCollection);
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

    private static void ConfigurePaymentGateway(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptionsWithValidateOnStart<MercadoPagoOptions>()
            .Bind(configuration.GetSection(MercadoPagoOptions.MercadoPago))
            .ValidateDataAnnotations();

        services.Configure<MercadoPagoOptions>(_ => configuration.GetSection(MercadoPagoOptions.MercadoPago))
            .AddSingleton<IMercadoPagoHmacSignatureValidator, MercadoPagoHmacSignatureValidator>()
            .AddScoped<IPaymentGateway, MercadoPagoService>()
            .AddKeyedScoped<IPaymentGateway, MercadoPagoService>($"Payment-{nameof(PaymentType.MercadoPago)}")
            .AddKeyedScoped<IPaymentGateway, FakePaymentGatewayService>($"Payment-{nameof(PaymentType.Test)}")
            .AddScoped<IPaymentGatewayFactoryMethod, PaymentGatewayFactory>();
    }

    private static void ConfigurePersistenceApp(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IDbConnection>(_ =>
        {
            DbProviderFactories.RegisterFactory("MySql.Data.MySqlClient", MySqlClientFactory.Instance);
            var providerFactory = DbProviderFactories.GetFactory("MySql.Data.MySqlClient");
            var conn = providerFactory.CreateConnection();
            conn.ConnectionString = configuration.GetConnectionString("MySql");
            conn.Open();
            return conn;
        });

        services.AddScoped<IOrderRepository, OrderRepositoryDapper>()
            .AddScoped<ICustomerRepository, CustomerRepositoryDapper>()
            .AddScoped<IProductRepository, ProductRepositoryDapper>()
            .AddScoped<IPaymentRepository, PaymentRepositoryDapper>();
    }
}
