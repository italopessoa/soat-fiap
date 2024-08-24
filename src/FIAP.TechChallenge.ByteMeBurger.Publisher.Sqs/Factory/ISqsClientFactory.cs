using Amazon.SQS;

namespace FIAP.TechChallenge.ByteMeBurger.Publisher.Sqs.Factory;

public interface ISqsClientFactory
{
    IAmazonSQS CreateClient();
}
