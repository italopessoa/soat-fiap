using Amazon;
using Amazon.SQS;
using FIAP.TechChallenge.ByteMeBurger.Publisher.Sqs.Factory;
using Microsoft.Extensions.Options;

namespace FIAP.TechChallenge.ByteMeBurger.Publisher.Sqs;

public class SqsClientFactory(IOptions<SqsSettings> sqsSettingsOptions) : ISqsClientFactory
{
    public IAmazonSQS CreateClient()
    {
        // TODO: https://github.com/soat-fiap/FIAP.TechChallenge.ByteMeBurger/issues/131
        return new AmazonSQSClient(RegionEndpoint.GetBySystemName(sqsSettingsOptions.Value.Region));
    }
}
