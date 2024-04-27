using System.Data;
using System.Data.Common;
using FIAP.TechChallenge.ByteMeBurger.Api.Configuration;
using FIAP.TechChallenge.ByteMeBurger.Application.Services;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Ingoing;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Outgoing;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;
using FIAP.TechChallenge.ByteMeBurger.Infrastructure.Repository;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace FIAP.TechChallenge.ByteMeBurger.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        // https://learn.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-8.0#log-automatic-400-responses
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers();

        builder.Services.AddScoped<ICustomerService, CustomerService>();
        builder.Services.AddScoped<IProductService, ProductService>();


        // builder.Services.AddSingleton<ICustomerRepository>(new InMemoryCustomerRepository(new[]
        // {
        //     new Customer("663.781.241-24", "Pietro Thales Anderson Rodrigues", "pietro_thales_rodrigues@silicotex.net")
        // }));

        builder.Services.AddScoped<ICustomerRepository, CustomerRepositoryDapper>();
        builder.Services.Configure<MySqlSettings>(builder.Configuration.GetSection(nameof(MySqlSettings)));
        builder.Services.AddSingleton<DbConnectionStringBuilder>(provider =>
        {
            var mySqlOptions = provider.GetService<IOptions<MySqlSettings>>();

            return new MySqlConnectionStringBuilder()
            {
                Server = mySqlOptions.Value.Server,
                Database = mySqlOptions.Value.Database,
                Port = mySqlOptions.Value.Port,
                Password = mySqlOptions.Value.Password,
                UserID = mySqlOptions.Value.UserId
            };
        });
        builder.Services.AddTransient<IDbConnection>(provider =>
        {
            DbProviderFactories.RegisterFactory("MySql.Data.MySqlClient", MySqlClientFactory.Instance);
            var builder = provider.GetRequiredService<DbConnectionStringBuilder>();
            var providerFactory = DbProviderFactories.GetFactory("MySql.Data.MySqlClient");
            var conn = providerFactory.CreateConnection();
            conn.ConnectionString = builder.ConnectionString;
            return conn;
        });


        builder.Services.AddSingleton<IProductRepository>(new InMemoryProductRepository(new[]
        {
            new Product("pao com ovo", "pao com ovo", ProductCategory.Meal, 2.5m, []),
            new Product("milkshake chocrante", "milkshake tijolo do bob'as", ProductCategory.SweatsNTreats, 2.5m, []),
            new Product("h20", "h20", ProductCategory.Beverage, 2.5m, []),
            new Product("batata frita", "batata frita", ProductCategory.FriesAndSides, 2.5m, [])
        }));

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();
        app.Run();
    }
}