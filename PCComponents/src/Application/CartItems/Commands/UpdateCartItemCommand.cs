using Application.CartItems.Exceptions;
using Application.Common;
using Application.Common.Interfaces.Repositories;
using Domain.CartItems;
using MediatR;

namespace Application.CartItems.Commands;

public record UpdateCartItemCommand : IRequest<Result<CartItem, CartItemException>>
{
    public required Guid CartItemId { get; init; }
    public required int Quantity { get; init; }
}

public class UpdateCartItemCommandHandler(
    ICartItemRepository cartItemRepository,
    IProductRepository productRepository) :
    IRequestHandler<UpdateCartItemCommand, Result<CartItem, CartItemException>>
{
    public async Task<Result<CartItem, CartItemException>> Handle(
        UpdateCartItemCommand request,
        CancellationToken cancellationToken)
    {
        var cartItemId = new CartItemId(request.CartItemId);
        var cartItemOption = await cartItemRepository.GetById(cartItemId, cancellationToken);

        return await cartItemOption.Match<Task<Result<CartItem, CartItemException>>>(
            async cartItem =>
            {
                if (cartItem.IsFinished)
                {
                    return await Task.FromResult<Result<CartItem, CartItemException>>(
                        new CartItemIsAlreadyFinishedException(cartItem.Id));
                }

                if (request.Quantity > cartItem.Product!.StockQuantity)
                {
                    return await Task.FromResult<Result<CartItem, CartItemException>>(
                        new CartItemQuantityExceedsStockException(cartItem.Product.Id, cartItem.Product.StockQuantity));
                }

                return await UpdateEntity(cartItem, request.Quantity, cancellationToken);
            },
            () => Task.FromResult<Result<CartItem, CartItemException>>(
                new CartItemNotFoundException(cartItemId)));
    }

    private async Task<Result<CartItem, CartItemException>> UpdateEntity(
        CartItem cartItem,
        int quantity,
        CancellationToken cancellationToken)
    {
        try
        {
            cartItem.UpdateQuantity(quantity);

            return await cartItemRepository.Update(cartItem, cancellationToken);
        }
        catch (Exception exception)
        {
            return new CartItemUnknownException(cartItem.Id, exception);
        }
    }
}