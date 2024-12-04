using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class OrderRepository(ApplicationDbContext context) : IOrderRepository, IOrderQueries, IStatusQueries
{
    public async Task<IReadOnlyList<Order>> GetAll(CancellationToken cancellationToken)
    {
        return await context.Orders
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Option<Order>> GetById(OrderId id, CancellationToken cancellationToken)
    {
        var entity = await context.Orders
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity == null ? Option.None<Order>() : Option.Some(entity);
    }

    public async Task<Order> Add(Order order, CancellationToken cancellationToken)
    {
        await context.Orders.AddAsync(order, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        return order;
    }

    public async Task<Order> Update(Order order, CancellationToken cancellationToken)
    {
        context.Orders.Update(order);
        await context.SaveChangesAsync(cancellationToken);
        return order;
    }

    public async Task<Order> Delete(Order order, CancellationToken cancellationToken)
    {
        context.Orders.Remove(order);
        await context.SaveChangesAsync(cancellationToken);
        return order;
    }

    public async Task<IReadOnlyList<Status>> GetAllStatuses(CancellationToken cancellationToken)
    {
        return await context.Statuses
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Option<Status>> GetStatusByName(string statusName, CancellationToken cancellationToken)
    {
        var entity = await context.Statuses
            .FirstOrDefaultAsync(x => x.Name == statusName, cancellationToken);

        return entity == null ? Option.None<Status>() : Option.Some(entity);
    }
}