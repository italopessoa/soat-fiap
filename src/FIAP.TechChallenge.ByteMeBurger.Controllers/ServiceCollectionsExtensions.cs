using System.Diagnostics.CodeAnalysis;
using FIAP.TechChallenge.ByteMeBurger.Controllers.Contracts;
using Bmb.Orders.Domain.Contracts;
using FIAP.TechChallenge.ByteMeBurger.Application.DomainServices;
using Microsoft.Extensions.DependencyInjection;

namespace FIAP.TechChallenge.ByteMeBurger.Controllers;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionsExtensions
{
    public static void AddControllers(this IServiceCollection services)
    {
        services.AddScoped<ICustomerService, CustomerService>()
            .AddScoped<IProductService, ProductService>()
            .AddScoped<IOrderService, OrderService>()
            .AddScoped<IOrderTrackingCodeService, OrderTrackingCodeService>();
    }
}
