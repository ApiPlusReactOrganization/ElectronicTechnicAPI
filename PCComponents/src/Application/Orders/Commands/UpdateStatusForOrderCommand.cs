using Application.Common;
using Application.Common.Interfaces.Queries;
using Application.Common.Interfaces.Repositories;
using Application.Orders.Exceptions;
using Domain.Orders;
using MediatR;

namespace Application.Orders.Commands;

public record UpdateStatusForOrderCommand : IRequest<Result<Order, OrderException>>
{
    public required Guid OrderId { get; init; }
    public required string StatusId { get; init; }
}

public class UpdateStatusForOrderCommandHandler(
    IOrderRepository orderRepository,
    IStatusQueries statusQueries)
    : IRequestHandler<UpdateStatusForOrderCommand, Result<Order, OrderException>>
{
    public async Task<Result<Order, OrderException>> Handle(UpdateStatusForOrderCommand request,
        CancellationToken cancellationToken)
    {
        var orderId = new OrderId(request.OrderId);
        var existingOrder = await orderRepository.GetById(orderId, cancellationToken);

        return await existingOrder.Match(
            async order =>
            {
                var existingStatus = await statusQueries.GetStatusByName(request.StatusId, cancellationToken);

                return await existingStatus.Match(
                    async status => await UpdateStatusForOrder(order, status.Id, cancellationToken),
                    () => Task.FromResult<Result<Order, OrderException>>(
                        new StatusNotFoundException(request.StatusId)));
            }, () => Task.FromResult<Result<Order, OrderException>>(
                new OrderNotFoundException(orderId)));
    }

    private async Task<Result<Order, OrderException>> UpdateStatusForOrder(Order order, string status,
        CancellationToken cancellationToken)
    {
        try
        {
            order.UpdateStatus(status);
            
            return await orderRepository.Update(order, cancellationToken);
        }
        catch (Exception exception)
        {
            return new OrderUnknownException(order.Id, exception);
        }
    }
}