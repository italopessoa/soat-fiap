using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Customers;

public class FindCustomerByCpfUseCase(ICustomerRepository customerRepository) : IFindCustomerByCpfUseCase
{
    public async Task<Customer?> Execute(string cpf)
    {
        return await customerRepository.FindByCpfAsync(cpf);
    }
}
