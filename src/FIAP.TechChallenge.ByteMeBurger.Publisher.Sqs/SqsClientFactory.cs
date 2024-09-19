using Amazon;
using Amazon.Runtime;
using Amazon.SQS;
using FIAP.TechChallenge.ByteMeBurger.Publisher.Sqs.Factory;
using Microsoft.Extensions.Options;

namespace FIAP.TechChallenge.ByteMeBurger.Publisher.Sqs;

public class SqsClientFactory(IOptions<SqsSettings> sqsSettingsOptions) : ISqsClientFactory
{
    public IAmazonSQS CreateClient()
    {
        // TODO: https://github.com/soat-fiap/FIAP.TechChallenge.ByteMeBurger/issues/131
        return new AmazonSQSClient(
            new BasicAWSCredentials(sqsSettingsOptions.Value.ClientId, sqsSettingsOptions.Value.ClientSecret),
            RegionEndpoint.GetBySystemName(sqsSettingsOptions.Value.Region));
    }
}
