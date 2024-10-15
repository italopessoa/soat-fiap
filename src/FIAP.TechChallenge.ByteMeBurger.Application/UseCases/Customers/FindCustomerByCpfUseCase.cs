using Bmb.Domain.Core.Entities;
using Bmb.Domain.Core.Interfaces;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Customers;

public class FindCustomerByCpfUseCase(ICustomerRepository customerRepository) : IFindCustomerByCpfUseCase
{
    public async Task<Customer?> Execute(string cpf)
    {
        return await customerRepository.FindByCpfAsync(cpf);
    }
}
