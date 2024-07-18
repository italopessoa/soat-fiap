using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Customers;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;

namespace FIAP.TechChallenge.ByteMeBurger.Application.Controllers;

public class CustomerService(
    ICreateCustomerUseCase createCustomerUseCase,
    IFindCustomerByCpfUseCase findCustomerByCpfUseCase) : ICustomerService
{
    public async Task<Customer?> FindByCpfAsync(string cpf)
    {
        return await findCustomerByCpfUseCase.Execute(cpf);
    }

    public async Task<Customer> CreateAsync(string cpf, string? name, string? email)
    {
        var customer = new Customer(cpf);
        if (name is not null)
            customer.ChangeName(name);

        if (email is not null)
            customer.ChangeEmail(email);

        return await createCustomerUseCase.Execute(cpf, name, email);
    }
}
