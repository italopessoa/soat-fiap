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

    public static implicit operator Customer(CustomerDto? dto) => ToDomain(dto);


    public static explicit operator CustomerDto(Customer? customer)
    {
        if (customer is null) return null;
        return new CustomerDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Email,
            Cpf = customer.Cpf
        };
    }

    private static Customer? ToDomain(CustomerDto? dto)
    {
        if (dto is null) return null;

        var customer = new Customer(dto.Id, dto.Cpf);
        if (dto.Email is not null) customer.ChangeEmail(dto.Email);
        if (dto.Name is not null) customer.ChangeName(dto.Name);
        return customer;
    }
}
