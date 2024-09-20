namespace FIAP.TechChallenge.ByteMeBurger.Publisher.Sqs;

/// <summary>
/// Sqs settings
/// </summary>
public class SqsSettings
{
    /// <summary>
    /// SQS Queue Name
    /// </summary>
    public string QueueName { get; set; } = string.Empty;

    /// <summary>
    /// Enabled
    /// </summary>
    public bool Enabled { get; set; } = false;

    /// <summary>
    /// AWS Region
    /// </summary>
    public string Region { get; set; } = string.Empty;

    /// <summary>
    /// AWS Secret Id
    /// </summary>
    public string ClientSecret { get; set; } = string.Empty;

    /// <summary>
    /// AWS Client Id
    /// </summary>
    public string ClientId { get; set; } = string.Empty;
}
