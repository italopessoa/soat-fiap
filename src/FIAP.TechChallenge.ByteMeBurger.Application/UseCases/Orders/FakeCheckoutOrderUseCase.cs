using System.Diagnostics.CodeAnalysis;
using FIAP.TechChallenge.ByteMeBurger.Domain.Base;
using FIAP.TechChallenge.ByteMeBurger.Domain.Events;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;

[ExcludeFromCodeCoverage(Justification = "fake use case")]
public class FakeCheckoutOrderUseCase : ICheckoutOrderUseCase
{
    private readonly IOrderRepository _orderRepository;

    public FakeCheckoutOrderUseCase(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task Execute(Guid orderId)
    {
        var order = await _orderRepository.GetAsync(orderId);
        if (order is null)
            throw new EntityNotFoundException("Order not found.");

        var oldStatus = order.Status;
        order.ConfirmPayment();

        try
        {
            await _orderRepository.UpdateOrderStatusAsync(order);
            DomainEventTrigger.RaiseOrderStatusChanged(order.Id, oldStatus, order.Status);
        }
        catch (Exception e)
        {
            throw new DomainException("Error when trying to confirm order payment.", e);
        }
    }
}
