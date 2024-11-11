using System.Text.Json;
using Amazon.SQS.Model;
using Bmb.Domain.Core.Entities;
using Bmb.Domain.Core.Events;
using Bmb.Domain.Core.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Masstransit.Factory;
using MassTransit;
using Microsoft.Extensions.Options;

namespace FIAP.TechChallenge.ByteMeBurger.Masstransit;


public interface IAnalyticsPublisher2
{
    Task PublishAsync<T>(T @event);
}

public class EventsDispatcher(IBus bus) : IAnalyticsPublisher2
{
    public Task PublishAsync<T>(DomainEvent<T> @event/* TODO: update domain CancellationToken cancellationToken*/)
    {
        return bus.Publish(new  OrderCreated(new Order()));
    }

    public Task PublishAsync<T>(T @event)
    {
        return bus.Publish(@event, default);
    }
}

public class SqsService(ISqsClientFactory sqsFactory, IOptions<SqsSettings> sqsSettingsOptions) : IAnalyticsPublisher
{
    public async Task PublishAsync<T>(DomainEvent<T> @event)
    {
        if (sqsSettingsOptions.Value.Enabled)
        {
            try
            {
                var client = sqsFactory.CreateClient();
                var queueUrl = await client.GetQueueUrlAsync(sqsSettingsOptions.Value.QueueName);
                var request = new SendMessageRequest
                {
                    MessageAttributes = new Dictionary<string, MessageAttributeValue>
                    {
                        {
                            "EventType", new MessageAttributeValue
                            {
                                DataType = "String",
                                StringValue = @event.GetType().Name
                            }
                        }
                    },
                    MessageBody = JsonSerializer.Serialize(@event.Payload),
                    QueueUrl = queueUrl.QueueUrl
                };

                await client.SendMessageAsync(request, default);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
