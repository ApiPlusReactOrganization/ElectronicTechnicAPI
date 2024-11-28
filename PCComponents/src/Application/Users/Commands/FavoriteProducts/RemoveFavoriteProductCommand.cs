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

        var userOption = await userRepository.GetById(userId, cancellationToken);
        var productOption = await productRepository.GetById(productId, cancellationToken);

        return await productOption.Match<Task<Result<User, UserException>>>(
            async product =>
            {
                return await userOption.Match<Task<Result<User, UserException>>>(
                    async user =>
                    {
                        if (!user.FavoriteProducts.Contains(product))
                        {
                            return await Task.FromResult<Result<User, UserException>>(
                                new UserFavoriteProductNotFoundException(user.Id, product.Id));
                        }

                        return await RemoveProductFromFavorites(userId, product, cancellationToken);
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
