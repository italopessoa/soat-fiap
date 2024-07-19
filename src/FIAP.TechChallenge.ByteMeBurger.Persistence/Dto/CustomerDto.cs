using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

namespace FIAP.TechChallenge.ByteMeBurger.Persistence.Dto;

public class CustomerDto
{
    public Guid Id { get; set; }

    public string Cpf { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public CustomerDto()
    {
    }
}
