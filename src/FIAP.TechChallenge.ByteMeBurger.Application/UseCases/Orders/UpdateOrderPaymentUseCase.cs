using FIAP.TechChallenge.ByteMeBurger.Domain.Base;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;

public class UpdateOrderPaymentUseCase(IOrderRepository repository) : IUpdateOrderPaymentUseCase
{
    public async Task<bool> Execute(Guid orderId, PaymentId paymentId)
    {
        var order = await repository.GetAsync(orderId);
        if (order == null)
        {
            throw new EntityNotFoundException($"Order '{orderId}' not found.");
        }

        order.SetPayment(paymentId);

        var updated = await repository.UpdateOrderPaymentAsync(order);
        return updated;
    }
}
