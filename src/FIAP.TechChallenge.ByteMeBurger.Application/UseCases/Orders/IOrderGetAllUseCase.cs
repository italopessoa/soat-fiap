using System.Collections.ObjectModel;
using FIAP.TechChallenge.ByteMeBurger.Domain.Entities;

namespace FIAP.TechChallenge.ByteMeBurger.Application.UseCases.Orders;

public interface IOrderGetAllUseCase
{
    Task<ReadOnlyCollection<Order>> Execute();
}