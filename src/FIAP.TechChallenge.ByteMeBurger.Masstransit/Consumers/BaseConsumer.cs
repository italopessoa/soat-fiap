using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace FIAP.TechChallenge.ByteMeBurger.Masstransit.Consumers;

public abstract class BaseConsumer<TEvent, TRequest, TResponse> : IConsumer<TEvent> where TEvent : class
{
    private readonly IUseCase<TRequest, TResponse> _useCase;
    protected TResponse Response;
    protected ILogger Logger { get; init; }

    protected BaseConsumer(ILogger logger, IUseCase<TRequest, TResponse> useCase)
    {
        Logger = logger;
        _useCase = useCase;
    }

    protected abstract TRequest PrepareRequest(TEvent request);

    public async Task Consume(ConsumeContext<TEvent> context)
    {
        Logger.LogInformation("Processing {EventType} event", context.Message.GetType());
        try
        {
            Response = await _useCase.ExecuteAsync(PrepareRequest(context.Message));
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error when processing {EventType} event", context.Message.GetType());
            throw;
        }
    }
}
