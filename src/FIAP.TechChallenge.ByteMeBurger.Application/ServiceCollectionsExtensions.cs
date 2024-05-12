using FIAP.TechChallenge.ByteMeBurger.Application.UseCases;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Customers;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Products;
using Microsoft.Extensions.DependencyInjection;

namespace FIAP.TechChallenge.ByteMeBurger.Application;

public static class ServiceCollectionsExtensions
{
    public static void AddUseCases(this IServiceCollection serviceCollection)
    {
        AddCustomerUseCases(serviceCollection);
        AddProductUseCases(serviceCollection);
        AddOrderUseCases(serviceCollection);
    }

    private static void AddCustomerUseCases(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IFindCustomerByCpfUseCase, FindCustomerByCpfUseCase>();
        serviceCollection.AddScoped<ICreateCustomerUseCase, CreateCustomerUseCase>();
    }

    private static void AddOrderUseCases(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ICreateOrderUseCase, CreateOrderUseCase>();
        serviceCollection.AddScoped<IOrderGetAllUseCase, OrderGetAllUseCase>();
        serviceCollection.AddScoped<IGetOrderDetailsUseCase, GetOrderDetailsUseCase>();
    }

    private static void AddProductUseCases(IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<ICreateProductUseCase, CreateProductUseCase>();
        serviceCollection.AddScoped<IFindProductsByCategoryUseCase, FindProductsByCategoryUseCase>();
        serviceCollection.AddScoped<IUpdateProductUseCase, UpdateProductUseCase>();
        serviceCollection.AddScoped<IGetAllProductsUseCase, GetAllProductsUseCase>();
        serviceCollection.AddScoped<IDeleteProductUseCase, DeleteProductUseCase>();
        serviceCollection.AddScoped<ICheckoutOrderUseCase, FakeCheckoutOrderUseCase>();
    }
}
