using System.Text.RegularExpressions;
using FIAP.TechChallenge.ByteMeBurger.Domain.Base;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

public partial class Customer : Entity<Guid>, IAggregateRoot
{
    public Cpf Cpf { get; private set; }

    public string? Name { get; private set; }

    public string? Email { get; private set; }

    public Customer()
        : base(Guid.NewGuid())
    {
    }

    public Customer(Guid id)
        : base(id)
    {
    }

    public Customer(Guid id, Cpf cpf)
        : base(id)
    {
        Cpf = cpf;
    }

    public Customer(Cpf cpf)
        : base(Guid.NewGuid())
    {
        Cpf = cpf;
    }

    public Customer(string cpf, string name, string email)
        : this(cpf)
    {
        Name = name;
        Email = email;
        Validate();
    }

    public Customer(Guid id, string cpf, string name, string email)
        : this(cpf)
    {
        Id = id;
        ChangeName(name);
        ChangeEmail(email);
    }

    private static string ValidateEmail(string? email)
    {
        AssertionConcern.AssertArgumentNotEmpty(email, nameof(email));
        if (!EmailRegex().IsMatch(email!))
            throw new DomainException($"Invalid email format '{email}'");

        return email!;
    }

    public void ChangeName(string name)
    {
        AssertionConcern.AssertArgumentNotEmpty(name, nameof(name));
        Name = name;
    }

    public void ChangeEmail(string email)
    {
        Email = ValidateEmail(email);
    }

    private void Validate()
    {
        AssertionConcern.AssertArgumentNotEmpty(Name, nameof(Name));
        ValidateEmail(Email);
    }

    [GeneratedRegex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$")]
    private static partial Regex EmailRegex();
}
