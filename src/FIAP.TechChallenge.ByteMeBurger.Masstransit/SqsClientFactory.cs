using Amazon.SQS;
using FIAP.TechChallenge.ByteMeBurger.Masstransit.Factory;
using Microsoft.Extensions.Options;

namespace FIAP.TechChallenge.ByteMeBurger.Masstransit;

public class SqsClientFactory(IOptions<SqsSettings> sqsSettingsOptions) : ISqsClientFactory
{
    public IAmazonSQS CreateClient()
    {
        // TODO: https://github.com/soat-fiap/FIAP.TechChallenge.ByteMeBurger/issues/131
        return new AmazonSQSClient();
    }
}
