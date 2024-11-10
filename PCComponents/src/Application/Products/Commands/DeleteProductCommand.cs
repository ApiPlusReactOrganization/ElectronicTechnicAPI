using Application.Products.Exceptions;
using Application.Common;
using Application.Common.Interfaces.Repositories;
using Domain.Products;
using MediatR;

namespace Application.Products.Commands;

public record DeleteProductCommand : IRequest<Result<Product, ProductException>>
{
    public required Guid ProductId { get; init; }
}

public class DeleteProductCommandHandler(
    IProductRepository productRepository)
    : IRequestHandler<DeleteProductCommand, Result<Product, ProductException>>
{
    public async Task<Result<Product, ProductException>> Handle(
        DeleteProductCommand request,
        CancellationToken cancellationToken)
    {
        var productId = new ProductId(request.ProductId);
        var existingProduct = await productRepository.GetById(productId, cancellationToken);

        return await existingProduct.Match<Task<Result<Product, ProductException>>>(
            async product => await DeleteEntity(product, cancellationToken),
            () => Task.FromResult<Result<Product, ProductException>>(new ProductNotFoundException(productId)));
    }

    private async Task<Result<Product, ProductException>> DeleteEntity(
        Product product,
        CancellationToken cancellationToken)
    {
        try
        {
            return await productRepository.Delete(product, cancellationToken);
        }
        catch (Exception exception)
        {
            return new ProductUnknownException(product.Id, exception);
        }
    }
}