using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Ingoing;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Outgoing;

namespace FIAP.TechChallenge.ByteMeBurger.Application.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;

    public CustomerService(ICustomerRepository repository)
    {
        _repository = repository;
    }


    public async Task<Customer?> FindByCpfAsync(string cpf)
    {
        return await _repository.FindByCpfAsync(cpf);
    }

    public async Task<Customer> CreateAnonymousAsync()
    {
        return await _repository.CreateAsync(new Customer());
    }

    public async Task<Customer> CreateAsync(string cpf)
    {
        return await _repository.CreateAsync(new Customer(cpf));
    }

    public async Task<Customer> CreateAsync(string cpf, string name, string email)
    {
        return await _repository.CreateAsync(new Customer(cpf, name, email));
    }
}