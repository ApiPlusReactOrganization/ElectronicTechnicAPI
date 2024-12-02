using Api.Dtos.Products;
using Domain.Authentications.Users;
using Domain.Products;

namespace Api.Dtos.Users;

public class UserFavoriteProductsDto
{
    public Guid UserId { get; set; }
    public List<ProductDto> FavoriteProducts { get; set; }

    public static UserFavoriteProductsDto FromDomainModel(User user)
    {
        return new UserFavoriteProductsDto
        {
            UserId = user.Id.Value,
            FavoriteProducts = user.FavoriteProducts.Select(ProductDto.FromDomainModel).ToList()
        };
    }
}
