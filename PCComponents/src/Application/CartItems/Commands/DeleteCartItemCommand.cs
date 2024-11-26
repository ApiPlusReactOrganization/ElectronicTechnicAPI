using Application.CartItems.Exceptions;
using Application.Common;
using Application.Common.Interfaces.Repositories;
using Domain.CartItems;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Application.CartItems.Commands;

public record DeleteCartItemCommand : IRequest<Result<CartItem, CartItemException>>
{
    public required Guid CartItemId { get; init; }
}

public class DeleteCartItemCommandHandler(
    ICartItemRepository cartItemRepository)
    : IRequestHandler<DeleteCartItemCommand, Result<CartItem, CartItemException>>
{
    public async Task<Result<CartItem, CartItemException>> Handle(
        DeleteCartItemCommand request,
        CancellationToken cancellationToken)
    {
        var cartItemId = new CartItemId(request.CartItemId);
        var existingCartItem = await cartItemRepository.GetById(cartItemId, cancellationToken);

        return await existingCartItem.Match<Task<Result<CartItem, CartItemException>>>(
            async cartItem => await DeleteEntity(cartItem, cancellationToken),
            () => Task.FromResult<Result<CartItem, CartItemException>>
                (new CartItemNotFoundException(cartItemId)));
    }

    private async Task<Result<CartItem, CartItemException>> DeleteEntity(
        CartItem cartItem,
        CancellationToken cancellationToken)
    {
        try
        {
            return await cartItemRepository.Delete(cartItem, cancellationToken);
        }
        catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23503")
        {
            return new CartItemHasRelatedEntitiesException(cartItem.Id);
        }
        catch (Exception exception)
        {
            return new CartItemUnknownException(cartItem.Id, exception);
        }
    }
}