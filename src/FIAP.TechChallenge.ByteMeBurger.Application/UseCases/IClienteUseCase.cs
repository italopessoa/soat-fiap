using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases
{
    public interface IClienteUseCase
    {
        Cliente GetByCpf(string cpf);
        void AddCliente(Cliente cliente);

    }
}
