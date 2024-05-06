using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Outgoing;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Customers;

public class FindCustomerByCpfUseCase : IFindCustomerByCpfUseCase
{
    private readonly ICustomerRepository _customerRepository;

    public FindCustomerByCpfUseCase(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    
    public async Task<Customer?> Execute(string cpf)
    {
        return await _customerRepository.FindByCpfAsync(cpf);
    }
}