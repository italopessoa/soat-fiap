using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Customers;
using FIAP.TechChallenge.ByteMeBurger.Controllers.Contracts;
using FIAP.TechChallenge.ByteMeBurger.Controllers.Dto;

namespace FIAP.TechChallenge.ByteMeBurger.Controllers;

public class CustomerService(
    ICreateCustomerUseCase createCustomerUseCase,
    IFindCustomerByCpfUseCase findCustomerByCpfUseCase) : ICustomerService
{
    public async Task<CustomerDto?> FindByCpfAsync(string cpf)
    {
        var customer = await findCustomerByCpfUseCase.Execute(cpf);
        return customer?.FromEntityToDto();
    }

    public async Task<CustomerDto> CreateAsync(string cpf, string? name, string? email)
    {
        var customer = new Domain.Entities.Customer(cpf);
        if (name is not null)
            customer.ChangeName(name);

        if (email is not null)
            customer.ChangeEmail(email);

        await createCustomerUseCase.Execute(cpf, name, email);
        return customer.FromEntityToDto();
    }
}
