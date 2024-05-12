using System.Diagnostics.CodeAnalysis;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Base;

[ExcludeFromCodeCoverage]
public class EntityNotFoundException : DomainException
{
    public EntityNotFoundException()
    {
    }

    public EntityNotFoundException(string message) : base(message)
    {
    }

    public EntityNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
