using Application.Categories.Exceptions;
using Application.Common;
using Application.Common.Interfaces.Repositories;
using Domain.Categories;
using MediatR;

namespace Application.Categories.Commands;

public record DeleteCategoryCommand : IRequest<Result<Category, CategoryException>>
{
    public required Guid CategoryId { get; init; }
}

public class DeleteCategoryCommandHandler(
    ICategoryRepository categoryRepository,
    IProductRepository productRepository)
    : IRequestHandler<DeleteCategoryCommand, Result<Category, CategoryException>>
{
    public async Task<Result<Category, CategoryException>> Handle(
        DeleteCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var categoryId = new CategoryId(request.CategoryId);
        var existingCategory = await categoryRepository.GetById(categoryId, cancellationToken);

        return await existingCategory.Match<Task<Result<Category, CategoryException>>>(
            async category => await DeleteEntity(category, cancellationToken),
            () => Task.FromResult<Result<Category, CategoryException>>
                (new CategoryNotFoundException(categoryId)));
    }

    private async Task<Result<Category, CategoryException>> DeleteEntity(
        Category category,
        CancellationToken cancellationToken)
    {
        try
        {
            var hasRelatedProducts = await productRepository.HasProductsInCategoryAsync(category.Id, cancellationToken);

            if (hasRelatedProducts)
            {
                return new CategoryHasRelatedProductsException(category.Id);
            }

            return await categoryRepository.Delete(category, cancellationToken);
        }
        catch (Exception exception)
        {
            return new CategoryUnknownException(category.Id, exception);
        }
    }

}