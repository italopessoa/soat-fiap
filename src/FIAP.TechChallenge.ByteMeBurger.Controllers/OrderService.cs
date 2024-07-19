using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;
using FIAP.TechChallenge.ByteMeBurger.Controllers.Contracts;
using FIAP.TechChallenge.ByteMeBurger.Controllers.Dto;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Controllers;

public class OrderService(
    ICreateOrderUseCase createOrderUseCase,
    IGetOrderDetailsUseCase getOrderDetailsUseCase,
    IOrderGetAllUseCase orderGetAllUseCase,
    IOrderRepository orderRepository,
    IUpdateOrderStatusUseCase updateOrderStatusUseCase,
    IUpdateOrderPaymentUseCase updateOrderPaymentUseCase)
    : IOrderService
{
    public async Task<NewOrderDto> CreateAsync(string? customerCpf, List<SelectedProduct> selectedProducts)
    {
        var cpf = customerCpf is not null ? new Cpf(customerCpf) : null;
        var order = await createOrderUseCase.Execute(cpf, selectedProducts);

        await orderRepository.CreateAsync(order);

        return order.FromEntityToCreatedDto();
    }

    public async Task<IReadOnlyCollection<OrderListItemDto>> GetAllAsync(bool listAll)
    {
        var orders = await orderGetAllUseCase.Execute(listAll);
        return orders.FromDomainToDto();
    }

    public async Task<OrderDetailDto?> GetAsync(Guid id)
    {
        var order = await getOrderDetailsUseCase.Execute(id);
        return order.FromEntityToDto();
    }

    public async Task<bool> UpdateStatusAsync(Guid orderId, OrderStatus newStatus)
    {
        return await updateOrderStatusUseCase.Execute(orderId, newStatus);
    }

    public async Task<bool> UpdateOrderPaymentAsync(Guid orderId, PaymentId paymentId)
    {
        return await updateOrderPaymentUseCase.Execute(orderId, paymentId);
    }
}
