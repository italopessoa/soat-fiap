using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.Json.Serialization;
using FIAP.TechChallenge.ByteMeBurger.Api.Configuration;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Serilog;

namespace FIAP.TechChallenge.ByteMeBurger.Api;

[ExcludeFromCodeCoverage]
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        try
        {
            builder.Host.UseSerilog((context, configuration) =>
                configuration.ReadFrom.Configuration(context.Configuration));

            // Add services to the container.
            builder.Services.AddAuthorization();
            builder.Services.AddSingleton<DomainEventsHandler>();
            builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));


            // Add CORS services to the DI container.
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            // https://learn.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-8.0#log-automatic-400-responses
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Tech Challenge Restaurant API", Version = "v1", Extensions =
                    {
                        {
                            "x-logo",
                            new OpenApiObject
                            {
                                {
                                    "url",
                                    new OpenApiString(
                                        "https://avatars.githubusercontent.com/u/165858718?s=384")
                                },
                                {
                                    "background",
                                    new OpenApiString(
                                        "#FF0000")
                                }
                            }
                        }
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            builder.Services.AddControllers(options => { options.Filters.Add<DomainExceptionFilter>(); })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });
            builder.Services.Configure<MySqlSettings>(builder.Configuration.GetSection(nameof(MySqlSettings)));
            builder.ConfigServicesDependencies();
            builder.Services.RegisterFacade();

            AddHealthChecks(builder);

            var app = builder.Build();
            app.Services.GetRequiredService<DomainEventsHandler>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseSerilogRequestLogging();
            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseHttpsRedirection();
            app.UseCors("AllowSpecificOrigins");
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application start-up failed");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static void AddHealthChecks(WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks()
            .AddMySql(provider => provider.GetRequiredService<DbConnectionStringBuilder>().ConnectionString);
    }
}
