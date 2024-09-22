using System.Diagnostics.CodeAnalysis;
using FIAP.TechChallenge.ByteMeBurger.Cognito.Gateway.Factory;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FIAP.TechChallenge.ByteMeBurger.Cognito.Gateway;

[ExcludeFromCodeCoverage]
public class CognitoSettingsSetup(IConfiguration configuration) : IConfigureOptions<CognitoSettings>
{
    public void Configure(CognitoSettings options)
    {
        configuration
            .GetSection(nameof(CognitoSettings))
            .Bind(options);
    }
}

[ExcludeFromCodeCoverage]
public static class ServiceExtensions
{
    public static void ConfigureCognito(this IServiceCollection services)
    {
        services.ConfigureOptions<CognitoSettingsSetup>();
        services.AddSingleton<ICognitoClientFactory, CognitoClientFactory>();
        services.AddScoped<ICustomerRepository, CognitoUserManager>();
    }
}
