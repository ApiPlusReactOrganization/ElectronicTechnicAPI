using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Products.Exceptions;
using Application.Services;
using Application.Services.ImageService;
using Domain.Products;
using MediatR;

namespace Application.Products.Commands;

public record DeleteProductImageCommand : IRequest<Result<Product, ProductException>>
{
    public Guid ProductId { get; init; }
    public Guid ProductImageId { get; init; }
}

public class DeleteProductImageCommandHandler(
    IProductRepository productRepository,
    IImageService imageService) : IRequestHandler<DeleteProductImageCommand, Result<Product, ProductException>>
{
    public async Task<Result<Product, ProductException>> Handle(DeleteProductImageCommand request,
        CancellationToken cancellationToken)
    {
        var productImageId = new ProductImageId(request.ProductImageId);
        var productId = new ProductId(request.ProductId);
        var existingProduct = await productRepository.GetById(productId, cancellationToken);

        return await existingProduct.Match(
            async product => await HandleImageDeletion(product, productImageId, cancellationToken),
            () => Task.FromResult<Result<Product, ProductException>>(
                new ProductNotFoundException(productId)));
    }

    private async Task<Result<Product, ProductException>> HandleImageDeletion(Product product,
        ProductImageId productImageId, CancellationToken cancellationToken)
    {
        var productImage = product.Images.FirstOrDefault(x => x.Id == productImageId);
        if (productImage is null)
        {
            return new ImageNotFoundException(productImageId);
        }

        var deleteResult = await imageService.DeleteImageAsync(ImagePaths.ProductImagesPath, productImage.FilePath);

        return await deleteResult.Match<Task<Result<Product, ProductException>>>(
            async _ =>
            {
                product.RemoveImage(productImageId);
                await productRepository.Update(product, cancellationToken);
                return product;
            },
            error => Task.FromResult<Result<Product, ProductException>>(new ImageSaveException(product.Id)));
    }
}