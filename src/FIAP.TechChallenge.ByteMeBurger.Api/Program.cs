using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using FIAP.TechChallenge.ByteMeBurger.Api.Configuration;
using FIAP.TechChallenge.ByteMeBurger.Application;
using FIAP.TechChallenge.ByteMeBurger.Application.Services;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Ingoing;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Outgoing;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
using FIAP.TechChallenge.ByteMeBurger.Infrastructure.Repository;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace FIAP.TechChallenge.ByteMeBurger.Api;

[ExcludeFromCodeCoverage]
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        // https://learn.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-8.0#log-automatic-400-responses
        builder.Services.AddUseCases();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers();
        builder.Services.Configure<MySqlSettings>(builder.Configuration.GetSection(nameof(MySqlSettings)));
        builder.ConfigServicesDependencies();
        builder.Services.RegisterServices();    

        builder.Services.AddHealthChecks()
            .AddMySql(provider => provider.GetRequiredService<DbConnectionStringBuilder>().ConnectionString);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter =  UIResponseWriter.WriteHealthCheckUIResponse
        });

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();
        app.Run();
    }

    
}