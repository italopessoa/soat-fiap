using System.Collections.ObjectModel;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Ingoing;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.Services;

public class OrderService(
    ICreateOrderUseCase createOrderUseCase,
    IGetOrderDetailsUseCase getOrderDetailsUseCase,
    IOrderGetAllUseCase orderGetAllUseCase,
    ICheckoutOrderUseCase checkoutOrderUseCase)
    : IOrderService
{
    public async Task<Order> CreateAsync(Cpf? customerCpf, List<(Guid productId, int quantity)> orderItems)
    {
        return await createOrderUseCase.Execute(customerCpf, orderItems);
    }

    public async Task<ReadOnlyCollection<Order>> GetAllAsync()
    {
        return await orderGetAllUseCase.Execute();
    }

    public async Task<Order?> GetAsync(Guid id)
    {
        return await getOrderDetailsUseCase.Execute(id);
    }

    public async Task CheckoutAsync(Guid id)
    {
        await checkoutOrderUseCase.Execute(id);
    }
}
