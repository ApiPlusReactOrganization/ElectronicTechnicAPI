using Domain.Products;

namespace Api.Dtos.Products;

public record ProductImageDto(Guid? Id, string FilePath)
{
    public static ProductImageDto FromDomainModel(ProductImage userImage)
        => new(userImage.Id.Value, userImage.FilePath);
}