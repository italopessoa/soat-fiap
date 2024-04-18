using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace FIAP.TechChallenge.ByteMeBurger.Infrastructure.Repositories
{
    public class ByteMeBurgerDbContext: DbContext
    {
        public ByteMeBurgerDbContext(DbContextOptions options) : base(options)
        {
           
        }
        public DbSet<Cliente> Clientes { get; set; }
    }
}
