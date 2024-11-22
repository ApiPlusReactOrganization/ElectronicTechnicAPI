using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Domain.Orders;
using Microsoft.EntityFrameworkCore;
using Optional;

namespace Infrastructure.Persistence.Repositories;

public class OrderRepository : IOrderRepository, IOrderQueries
{
    private readonly ApplicationDbContext _context;

    public OrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Order>> GetAll(CancellationToken cancellationToken)
    {
        return await _context.Orders
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Option<Order>> GetById(OrderId id, CancellationToken cancellationToken)
    {
        var entity = await _context.Orders
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity == null ? Option.None<Order>() : Option.Some(entity);
    }

    public async Task<Order> Add(Order order, CancellationToken cancellationToken)
    {
        await _context.Orders.AddAsync(order, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return order;
    }

    public async Task<Order> Update(Order order, CancellationToken cancellationToken)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync(cancellationToken);
        return order;
    }

    public async Task<Order> Delete(Order order, CancellationToken cancellationToken)
    {
        _context.Orders.Remove(order);
        await _context.SaveChangesAsync(cancellationToken);
        return order;
    }
}