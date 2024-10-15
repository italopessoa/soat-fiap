using System.Diagnostics.CodeAnalysis;
using Bmb.Domain.Core.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Publisher.Sqs.Factory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FIAP.TechChallenge.ByteMeBurger.Publisher.Sqs;

[ExcludeFromCodeCoverage]
public class SqsSettingsSetup(IConfiguration configuration) : IConfigureOptions<SqsSettings>
{
    public void Configure(SqsSettings options)
    {
        configuration
            .GetSection(nameof(SqsSettings))
            .Bind(options);
    }
}

[ExcludeFromCodeCoverage]
public static class ServiceExtensions
{
    public static void ConfigureAnalyticsService(this IServiceCollection services)
    {
        services.ConfigureOptions<SqsSettingsSetup>();
        services.AddSingleton<ISqsClientFactory, SqsClientFactory>();
        services.AddSingleton<IAnalyticsPublisher, SqsService>();
    }
}
