namespace FIAP.TechChallenge.ByteMeBurger.Publisher.Sqs;

public class SqsSettings
{
    public string QueueName { get; set; } = null!;

    public bool Enabled { get; set; } = false;

    public string Region { get; set; } = null!;
}
