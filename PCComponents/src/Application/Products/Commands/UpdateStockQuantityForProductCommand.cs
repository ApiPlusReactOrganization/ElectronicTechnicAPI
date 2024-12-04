using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Products.Exceptions;
using Domain.Products;
using MediatR;

namespace Application.Products.Commands;

public record UpdateStockQuantityForProductCommand : IRequest<Result<Product, ProductException>>
{
    public required Guid ProductId { get; init; }
    public required int StockQuantity { get; init; }
}

public class UpdateStockQuantityForProductCommandHandler(IProductRepository productRepository)
    : IRequestHandler<UpdateStockQuantityForProductCommand,
        Result<Product, ProductException>>
{
    public async Task<Result<Product, ProductException>> Handle(UpdateStockQuantityForProductCommand request,
        CancellationToken cancellationToken)
    {
        var productId = new ProductId(request.ProductId);
        var existingProduct = await productRepository.GetById(productId, cancellationToken);

        return await existingProduct.Match(
            async p => await UpdateStockQuantity(p, request.StockQuantity, cancellationToken),
            () => Task.FromResult<Result<Product, ProductException>>(
                new ProductNotFoundException(productId)));
    }

    private async Task<Result<Product, ProductException>> UpdateStockQuantity(Product product, int requestStockQuantity,
        CancellationToken cancellationToken)
    {
        try
        {
            product.SetStockQuantity(requestStockQuantity);
            
            return await productRepository.Update(product, cancellationToken);
        }
        catch (Exception exception)
        {
            return new ProductUnknownException(product.Id, exception);
        }
    }
}