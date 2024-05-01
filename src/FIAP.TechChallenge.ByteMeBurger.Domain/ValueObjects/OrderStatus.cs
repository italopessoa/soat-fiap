namespace FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

public enum OrderStatus
{
    PaymentPending = 0,
    Received,
    Preparing,
    Done,
    Finished
}