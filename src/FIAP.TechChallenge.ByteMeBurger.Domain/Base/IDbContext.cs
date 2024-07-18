using System.Data;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Base;

public interface IDbContext
{
    IDbConnection CreateConnection();
}
