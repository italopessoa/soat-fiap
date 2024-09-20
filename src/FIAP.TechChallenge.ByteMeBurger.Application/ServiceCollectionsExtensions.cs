using System.Diagnostics.CodeAnalysis;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Customers;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Payment;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Products;
using Microsoft.Extensions.DependencyInjection;

namespace FIAP.TechChallenge.ByteMeBurger.Application;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionsExtensions
{
    public static void AddUseCases(this IServiceCollection serviceCollection)
    {
        AddCustomerUseCases(serviceCollection);
        AddOrderUseCases(serviceCollection);
        AddProductUseCases(serviceCollection);
        AddPaymentUseCases(serviceCollection);
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
}
