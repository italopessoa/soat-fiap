using System.Collections.ObjectModel;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Ingoing;
using FIAP.TechChallenge.ByteMeBurger.Domain.Ports.Outgoing;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.Services;

public class OrderService(
    ICheckoutOrderUseCase checkoutOrderUseCase,
    IGetOrderDetailsUseCase getOrderDetailsUseCase,
    IOrderGetAllUseCase orderGetAllUseCase)
    : IOrderService
{
    public async Task<Order> CreateAsync(Cpf? customerCpf,
        List<(Guid productId, string productName, int quantity, decimal unitPrice)> orderItems)
    {
        return await checkoutOrderUseCase.Execute(customerCpf, orderItems);
    }

    public async Task<ReadOnlyCollection<Order>> GetAllAsync()
    {
        return await orderGetAllUseCase.Execute();
    }

    public async Task<Order?> GetAsync(Guid id)
    {
        return await getOrderDetailsUseCase.Execute(id);
    }
}