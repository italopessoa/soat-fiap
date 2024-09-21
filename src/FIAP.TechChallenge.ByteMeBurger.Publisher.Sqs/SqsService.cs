using System.Text.Json;
using Amazon.SQS.Model;
using FIAP.TechChallenge.ByteMeBurger.Domain.Events;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Publisher.Sqs.Factory;
using Microsoft.Extensions.Options;

namespace FIAP.TechChallenge.ByteMeBurger.Publisher.Sqs;

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
