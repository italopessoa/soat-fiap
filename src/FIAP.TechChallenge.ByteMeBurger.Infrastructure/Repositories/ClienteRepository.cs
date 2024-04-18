using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Repositories;

namespace FIAP.TechChallenge.ByteMeBurger.Infrastructure.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ByteMeBurgerDbContext _context;
        public ClienteRepository(ByteMeBurgerDbContext context) { 
            _context = context;
        }
        public Task<Cliente> GetByCpf(string cpf)
        {
            throw new NotImplementedException();
        }
    }
}
