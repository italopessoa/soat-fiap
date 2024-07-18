namespace FIAP.TechChallenge.ByteMeBurger.Api.Model.Orders;

public record OrderListItemDto(
    Guid Id,
    string TrackingCode,
    decimal Total,
    OrderStatusDto Status,
    DateTime CreatedAt,
    DateTime? UpdatedAt);
