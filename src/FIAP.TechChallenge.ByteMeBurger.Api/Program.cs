using FIAP.TechChallenge.ByteMeBurger.Application.UseCases;
using FIAP.TechChallenge.ByteMeBurger.Domain.Repositories;

namespace FIAP.TechChallenge.ByteMeBurger.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers();

        #region Injeção de dependências

        //builder.Services.AddTransient<IClienteRepository, ClienteRepository>();
        builder.Services.AddTransient<IClienteUseCase, ClienteUseCase>();
        #endregion

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        // if (app.Environment.IsDevelopment())
        // {
            app.UseSwagger();
            app.UseSwaggerUI();
        // }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        app.MapControllers();
        app.Run();
    }
}