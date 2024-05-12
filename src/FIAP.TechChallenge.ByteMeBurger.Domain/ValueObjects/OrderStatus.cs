namespace FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

/// <summary>
/// Orders statuses
/// </summary>
public enum OrderStatus
{
    PaymentPending = 0,
    PaymentConfirmed = 1,
    Received = 2,
    InPreparation = 3,
    Ready = 4,
    Completed = 5
}
