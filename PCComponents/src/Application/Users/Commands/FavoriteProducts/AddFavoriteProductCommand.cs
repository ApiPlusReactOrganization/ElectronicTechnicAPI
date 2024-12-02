using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Users.Exceptions;
using Domain.Authentications.Users;
using Domain.Products;
using MediatR;
using ProductNotFoundException = Application.Users.Exceptions.ProductNotFoundException;

namespace Application.Users.Commands.FavoriteProducts;

public record AddFavoriteProductCommand : IRequest<Result<User, UserException>>
{
    public required Guid UserId { get; init; }
    public required Guid ProductId { get; init; }
}

public class AddFavoriteProductCommandHandler(
    IUserRepository userRepository,
    IProductRepository productRepository)
    : IRequestHandler<AddFavoriteProductCommand, Result<User, UserException>>
{
    public async Task<Result<User, UserException>> Handle(
        AddFavoriteProductCommand request,
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
                        if (user.FavoriteProducts.Contains(product))
                        {
                            return await Task.FromResult<Result<User, UserException>>(
                                new ProductAlreadyInFavoritesException(user.Id, product.Id));
                        }
                        return await AddProductToFavorites(userId, product, cancellationToken);
                    },
                    () => Task.FromResult<Result<User, UserException>>(
                        new UserNotFoundException(userId))
                );
            },
            () => Task.FromResult<Result<User, UserException>>(
                new ProductNotFoundException(productId))
        );
    }

    private async Task<Result<User, UserException>> AddProductToFavorites(
        UserId userId,
        Product product,
        CancellationToken cancellationToken)
    {
        try
        {
            return await userRepository.AddFavoriteProduct(userId, product, cancellationToken);
        }
        catch (Exception exception)
        {
            return new UserUnknownException(userId, exception);
        }
    }
}

