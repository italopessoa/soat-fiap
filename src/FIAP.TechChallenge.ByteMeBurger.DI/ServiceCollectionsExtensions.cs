using System.Diagnostics.CodeAnalysis;
using FIAP.TechChallenge.ByteMeBurger.Application;
using FIAP.TechChallenge.ByteMeBurger.Cognito.Gateway;
using FIAP.TechChallenge.ByteMeBurger.Controllers;
using FIAP.TechChallenge.ByteMeBurger.Persistence;
using FIAP.TechChallenge.ByteMeBurger.Masstransit;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FIAP.TechChallenge.ByteMeBurger.DI;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionsExtensions
{
    public static void IoCSetup(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.ConfigureCognito();
        serviceCollection.ConfigurePersistenceApp(configuration);
        ConfigHybridCache(serviceCollection, configuration);
        serviceCollection.AddUseCases();
        serviceCollection.AddControllers();
        serviceCollection.ConfigureAnalyticsService();
        serviceCollection.ConfigureDispatcher();
    }

    private static void ConfigHybridCache(IServiceCollection services, IConfiguration configuration)
    {
        var hybridCacheSettings = configuration.GetSection("HybridCache")
            .Get<HybridCacheEntryOptions>();
        services.AddHybridCache(options =>
            options.DefaultEntryOptions = new HybridCacheEntryOptions()
            {
                Expiration = hybridCacheSettings!.Expiration,
                LocalCacheExpiration = hybridCacheSettings.LocalCacheExpiration,
                Flags = hybridCacheSettings.Flags,
            }
        );
    }
}
