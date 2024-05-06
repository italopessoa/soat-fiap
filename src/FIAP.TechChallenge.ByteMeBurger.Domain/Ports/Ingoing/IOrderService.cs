using System.Collections.ObjectModel;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Ingoing;

public interface IOrderService
{
    Task<Order> CreateAsync(Cpf? customerCpf, List<(Guid productId, int quantity)> orderItems);
    
    Task<ReadOnlyCollection<Order>> GetAllAsync();
    
    Task<Order?> GetAsync(Guid id);
}