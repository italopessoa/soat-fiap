namespace FIAP.TechChallenge.ByteMeBurger.Domain.Base;

public class UseCaseException : DomainException
{
    public UseCaseException()
    {
    }

    public UseCaseException(string message) : base(message)
    {
    }

    public UseCaseException(string message, Exception innerException) : base(message, innerException)
    {
    }
}