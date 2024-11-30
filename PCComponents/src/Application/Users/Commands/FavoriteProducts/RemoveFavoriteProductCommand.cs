using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Users.Exceptions;
using Domain.Authentications.Users;
using Domain.Products;
using MediatR;

namespace Application.Users.Commands.FavoriteProducts;

public record RemoveFavoriteProductCommand : IRequest<Result<User, UserException>>
{
    public required Guid UserId { get; init; }
    public required Guid ProductId { get; init; }
}

public class RemoveFavoriteProductCommandHandler(
    IUserRepository userRepository,
    IProductRepository productRepository)
    : IRequestHandler<RemoveFavoriteProductCommand, Result<User, UserException>>
{
    public async Task<Result<User, UserException>> Handle(
        RemoveFavoriteProductCommand request,
        CancellationToken cancellationToken)
    {
        var userId = new UserId(request.UserId);
        var productId = new ProductId(request.ProductId);

        var user = await userRepository.GetById(userId, cancellationToken);
        var product = await productRepository.GetById(productId, cancellationToken);

        return await product.Match<Task<Result<User, UserException>>>(
            async p =>
            {
                return await user.Match<Task<Result<User, UserException>>>(
                    async u =>
                    {
                        if (!u.FavoriteProducts.Contains(p))
                        {
                            return await Task.FromResult<Result<User, UserException>>(
                                new UserFavoriteProductNotFoundException(userId, productId));
                        }

                        return await RemoveProductFromFavorites(userId, p, cancellationToken);
                    },
                    () => Task.FromResult<Result<User, UserException>>(
                        new UserNotFoundException(userId))
                );
            },
            () => Task.FromResult<Result<User, UserException>>(
                new ProductNotFoundException(productId))
        );
    }

    private async Task<Result<User, UserException>> RemoveProductFromFavorites(
        UserId userId,
        Product product,
        CancellationToken cancellationToken)
    {
        try
        {
            return await userRepository.RemoveFavoriteProduct(userId, product, cancellationToken);
        }
        
        catch (Exception exception)
        {
            return new UserUnknownException(UserId.Empty, exception);
        }
    }
}
