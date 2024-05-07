namespace FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

/// <summary>
/// Orders statuses
/// </summary>
public enum OrderStatus
{
    PaymentPending = 0,
    Received,
    Preparing,
    Done,
    Finished
}