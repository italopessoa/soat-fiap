using Amazon.SQS;

namespace FIAP.TechChallenge.ByteMeBurger.Masstransit.Factory;

public interface ISqsClientFactory
{
    IAmazonSQS CreateClient();
}
