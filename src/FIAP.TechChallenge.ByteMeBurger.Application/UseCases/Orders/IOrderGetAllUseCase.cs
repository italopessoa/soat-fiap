using System.Collections.ObjectModel;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;

public interface IOrderGetAllUseCase
{
    /// <summary>
    /// Get all Orders
    /// </summary>
    /// <param name="listAll">If true it will return all orders. If false it returns only orders
    /// with status (Received, In Preparation or Ready).</param>
    /// <returns>Orders</returns>
    Task<ReadOnlyCollection<Order>> Execute(bool listAll);
}
