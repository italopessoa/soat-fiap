using FIAP.TechChallenge.ByteMeBurger.Domain.Base;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Events;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Outgoing;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Customers;

public class CreateCustomerUseCase(ICustomerRepository customerRepository) : ICreateCustomerUseCase
{
    public async Task<Customer> Execute(Cpf cpf, string? name, string? email)
    {
        if (await customerRepository.FindByCpfAsync(cpf.Value) is not null)
        {
            throw new UseCaseException("There's already a customer using the provided CPF value.");
        }

        var customer = new Customer(cpf);
        if (name is not null)
            customer.ChangeName(name);

        if (email is not null)
            customer.ChangeEmail(email);

        var newCustomer = await customerRepository.CreateAsync(customer);
        if (newCustomer is null)
            throw new UseCaseException("An error occurred while trying to create the customer.");

        DomainEventTrigger.RaiseCustomerRegistered(new CustomerRegistered(customer));
        return newCustomer;
    }
}