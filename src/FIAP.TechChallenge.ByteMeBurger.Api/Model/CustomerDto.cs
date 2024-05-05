using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

namespace FIAP.TechChallenge.ByteMeBurger.Api.Model;

public class CustomerDto
{
    public Guid Id { get; set; }

    public string Cpf { get; set; }
    
    public string? Name { get; set; }

    public string? Email { get; set; }

    public CustomerDto()
    {
    }

    public CustomerDto(Customer customer)
    {
        Id = customer.Id;
        Cpf = customer.Cpf;
        Name = customer.Name;
        Email = customer.Email;
    }
}