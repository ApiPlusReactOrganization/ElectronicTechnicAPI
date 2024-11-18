using Application.Common;
using Application.Common.Interfaces.Repositories;
using Application.Products.Exceptions;
using Application.Services;
using Application.Services.ImageService;
using Domain.Products;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Products.Commands;

public class UploadProductImagesCommand : IRequest<Result<Product, ProductException>>
{
    public Guid ProductId { get; init; }
    public IFormFileCollection ImagesFiles { get; init; }
}

public class UploadProductImagesCommandHandler(
    IProductRepository productRepository,
    IImageService imageService) : IRequestHandler<UploadProductImagesCommand, Result<Product, ProductException>>
{
    public async Task<Result<Product, ProductException>> Handle(UploadProductImagesCommand request,
        CancellationToken cancellationToken)
    {
        var productId = new ProductId(request.ProductId);
        var existingProduct = await productRepository.GetById(productId, cancellationToken);

        return await existingProduct.Match(
            async product => await UploadOrReplaceImage(product, request.ImagesFiles, cancellationToken),
            () => Task.FromResult<Result<Product, ProductException>>(
                new ProductNotFoundException(productId)));
    }
    
    private async Task<Result<Product, ProductException>> UploadOrReplaceImage(
        Product product,
        IFormFileCollection imagesFiles,
        CancellationToken cancellationToken)
    {
        var imageSaveResult = await imageService.SaveImagesFromFilesAsync(ImagePaths.ProductImagesPath, imagesFiles, product.Images);

        return await imageSaveResult.Match<Task<Result<Product, ProductException>>>(
            async imagesNames =>
            {
                var imagesEntities = new List<ProductImage>();

                foreach (var imageName in imagesNames)
                {
                    imagesEntities.Add(ProductImage.New(ProductImageId.New(), product.Id, imageName));
                }
                
                product.UpdateProductImage(imagesEntities);
                
                var productWithImages = await productRepository.Update(product, cancellationToken);
                return productWithImages;
            },
            () => Task.FromResult<Result<Product, ProductException>>(new ImageSaveException(product.Id)));
    }
}