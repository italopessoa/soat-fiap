using System.Diagnostics.CodeAnalysis;
using Bmb.Domain.Core.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Customers;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Products;
using Microsoft.Extensions.DependencyInjection;

namespace FIAP.TechChallenge.ByteMeBurger.Application;

[ExcludeFromCodeCoverage]
public static class ServiceExtensions
{
    public static void AddUseCases(this IServiceCollection serviceCollection)
    {
        AddCustomerUseCases(serviceCollection);
        AddOrderUseCases(serviceCollection);
        AddProductUseCases(serviceCollection);
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
            .AddScoped<IUseCase<UpdateOrderStatusRequest, bool>, UpdateOrderStatusUseCase>()
            .AddScoped<IUpdateOrderPaymentUseCase, UpdateOrderPaymentUseCase>();
    }

    private static void AddProductUseCases(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ICreateProductUseCase, CreateProductUseCase>()
            .AddScoped<IFindProductsByCategoryUseCase, FindProductsByCategoryUseCase>()
            .AddScoped<IUpdateProductUseCase, UpdateProductUseCase>()
            .AddScoped<IGetAllProductsUseCase, GetAllProductsUseCase>()
            .AddScoped<IDeleteProductUseCase, DeleteProductUseCase>();
    }
}
