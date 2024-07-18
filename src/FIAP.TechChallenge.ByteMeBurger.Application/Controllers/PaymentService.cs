using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;
using FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Payment;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;
using FIAP.TechChallenge.ByteMeBurger.Domain.Interfaces;
using FIAP.TechChallenge.ByteMeBurger.Domain.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.Controllers;

public class PaymentService : IPaymentService
{
    private readonly ICreatePaymentUseCase _createOrderPaymentUseCase;
    private readonly IUpdatePaymentStatusUseCase _updatePaymentStatusUseCase;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IPaymentGatewayFactoryMethod _paymentGatewayFactory;
    private readonly IUpdateOrderPaymentUseCase _updateOrderPaymentUseCase;

    public PaymentService(ICreatePaymentUseCase createOrderPaymentUseCase,
        IUpdatePaymentStatusUseCase updatePaymentStatusUseCase,
        IPaymentRepository paymentRepository,
        IPaymentGatewayFactoryMethod paymentGatewayFactory,
        IUpdateOrderPaymentUseCase updateOrderPaymentUseCase)
    {
        _createOrderPaymentUseCase = createOrderPaymentUseCase;
        _updatePaymentStatusUseCase = updatePaymentStatusUseCase;
        _paymentRepository = paymentRepository;
        _paymentGatewayFactory = paymentGatewayFactory;
        _updateOrderPaymentUseCase = updateOrderPaymentUseCase;
    }

    public async Task<Payment> CreateOrderPaymentAsync(CreateOrderPaymentRequestDto command)
    {
        var payment = await _createOrderPaymentUseCase.Execute(command.OrderId, command.PaymentType);
        if (payment is null)
            return null;

        await _paymentRepository.SaveAsync(payment);
        await _updateOrderPaymentUseCase.Execute(payment.OrderId, payment.Id);
        return payment;
    }

    public async Task<Payment?> GetPaymentAsync(PaymentId paymentId)
    {
        return await _paymentRepository.GetPaymentAsync(paymentId);
    }

    public async Task<Payment?> GetPaymentAsync(string paymentId, PaymentType paymentType)
    {
        return await _paymentRepository.GetPaymentAsync(paymentId, paymentType);
    }

    public async Task<bool> SyncPaymentStatusWithGatewayAsync(string externalReference, PaymentType paymentType)
    {
        var payment = await GetPaymentAsync(externalReference, paymentType);
        if (payment is null)
            return false;

        var paymentGateway = _paymentGatewayFactory.Create(paymentType);
        var paymentStatus = await paymentGateway.GetPaymentStatusAsync(externalReference);
        if (paymentStatus is null)
            return false;

        await _updatePaymentStatusUseCase.Execute(payment, paymentStatus.Value);
        return true;
    }
}
