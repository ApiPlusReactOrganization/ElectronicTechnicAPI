using Application.CartItems.Exceptions;
using Application.Common;
using Application.Common.Interfaces.Repositories;
using Domain.Authentications.Users;
using Domain.CartItems;
using Domain.Products;
using MediatR;

namespace Application.CartItems.Commands;

public record CreateCartItemCommand : IRequest<Result<CartItem, CartItemException>>
{
    public required Guid UserId { get; init; }
    public required Guid ProductId { get; init; }
    public required int Quantity { get; init; }
}

public class CreateCartItemCommandHandler(
    ICartItemRepository cartItemRepository,
    IProductRepository productRepository,
    IUserRepository userRepository)
    : IRequestHandler<CreateCartItemCommand, Result<CartItem, CartItemException>>
{
    public async Task<Result<CartItem, CartItemException>> Handle(
        CreateCartItemCommand request,
        CancellationToken cancellationToken)
    {
        var productId = new ProductId(request.ProductId);
        var product = await productRepository.GetById(productId, cancellationToken);

        return await product.Match<Task<Result<CartItem, CartItemException>>>(
            async p =>
            {
                if (request.Quantity > p.StockQuantity)
                {
                    return await Task.FromResult<Result<CartItem, CartItemException>>(
                        new CartItemQuantityExceedsStockException(productId, p.StockQuantity));
                }

                var userId = new UserId(request.UserId);
                var user = await userRepository.GetById(userId, cancellationToken);

                return await user.Match<Task<Result<CartItem, CartItemException>>>(
                    async c =>
                    {
                        var existingCartItem = await cartItemRepository.GetByProduct(
                            productId,
                            cancellationToken);

                        return await existingCartItem.Match<Task<Result<CartItem, CartItemException>>>(
                            ci => Task.FromResult<Result<CartItem, CartItemException>>(
                                new CartItemAlreadyExistsException(ci.Id)),
                            async () => await CreateEntity(userId, productId, request.Quantity, cancellationToken));
                    },
                    () => Task.FromResult<Result<CartItem, CartItemException>>(
                        new CartItemUserNotFoundException(userId)));
            },
            () => Task.FromResult<Result<CartItem, CartItemException>>(
                new CartItemProductNotFoundException(productId)));
    }


    private async Task<Result<CartItem, CartItemException>> CreateEntity(
        UserId userId,
        ProductId productId,
        int quantity,
        CancellationToken cancellationToken)
    {
        try
        {
            var entity = CartItem.New(CartItemId.New(), userId, productId, quantity);

            return await cartItemRepository.Add(entity, cancellationToken);
        }
        catch (Exception exception)
        {
            return new CartItemUnknownException(CartItemId.Empty, exception);
        }
    }
}