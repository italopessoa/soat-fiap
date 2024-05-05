using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Outgoing;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;

public class GetOrderDetailsUseCase(IOrderRepository repository) : IGetOrderDetailsUseCase
{
    public async Task<Order?> Execute(Guid id)
    {
        return id == Guid.Empty ? null : await repository.GetAsync(id);
    }
}