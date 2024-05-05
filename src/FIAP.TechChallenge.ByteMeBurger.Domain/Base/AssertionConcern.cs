namespace FIAP.TechChallenge.ByteMeBurger.Domain.Base;

public static class AssertionConcern
{
    public static void AssertArgumentNotEmpty(string value, string arg)
    {
        if (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException($"{arg} cannot be empty.");
        }
    }

    public static void AssertArgumentNotZeroOrNegative(decimal value, string arg)
    {
        if (value <= 0)
        {
            throw new DomainException($"{arg} must be greater than 0");
        }
    }
}