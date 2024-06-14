using FIAP.TechChallenge.ByteMeBurger.Application.DomainServices;
using FIAP.TechChallenge.ByteMeBurger.Application.Services;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Customers;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Payment;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Products;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FIAP.TechChallenge.ByteMeBurger.Application;

public static class ServiceCollectionsExtensions
{
    public static void ConfigureApplicationApp(this IServiceCollection serviceCollection)
    {
        AddCustomerUseCases(serviceCollection);
        AddProductUseCases(serviceCollection);
        AddOrderUseCases(serviceCollection);
        AddPaymentUseCases(serviceCollection);
        RegisterServices(serviceCollection);
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
            .AddScoped<IDeleteProductUseCase, DeleteProductUseCase>()
            .AddScoped<ICheckoutOrderUseCase, FakeCheckoutOrderUseCase>();
    }

    private static void AddPaymentUseCases(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ICreatePaymentUseCase, CreatePaymentUseCase>();
    }

    private static void RegisterServices(IServiceCollection services)
    {
        services.AddScoped<ICustomerService, CustomerService>()
            .AddScoped<IProductService, ProductService>()
            .AddScoped<IOrderService, OrderService>()
            .AddScoped<IOrderTrackingCodeService, OrderTrackingCodeService>()
            .AddScoped<IPaymentService, PaymentService>();
    }
}
