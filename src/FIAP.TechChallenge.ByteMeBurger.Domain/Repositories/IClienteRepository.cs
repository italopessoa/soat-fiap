using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Repositories
{
    public interface IClienteRepository
    {
        Task<Cliente> GetByCpf(string cpf);

    }
}
