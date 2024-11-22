using Domain.Orders;
using Optional;

namespace Application.Common.Interfaces.Repositories;

public interface IOrderRepository
{
    Task<Option<Order>> GetById(OrderId id, CancellationToken cancellationToken);
    /*
    Task<Option<CartItem>> SearchByName(string name, CancellationToken cancellationToken);
    */
    Task<Order> Add(Order сategory, CancellationToken cancellationToken);
    Task<Order> Update(Order сategory, CancellationToken cancellationToken);
    Task<Order> Delete(Order сategory, CancellationToken cancellationToken);
}