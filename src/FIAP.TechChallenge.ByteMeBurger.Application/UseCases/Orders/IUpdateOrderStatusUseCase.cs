using Bmb.Domain.Core.ValueObjects;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;

public record UpdateOrderStatusRequest(Guid OrderId, OrderStatus NewStatus);
