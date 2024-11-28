using Api.Dtos.Products;
using Domain.Products;

namespace Api.Dtos.Users
{
    public class UserFavoriteProductsListDto
    {
        public Guid UserId { get; set; }
        public List<ProductDto> FavoriteProducts { get; set; }
        
        public static UserFavoriteProductsListDto FromProductList(Guid userId, List<Product> favoriteProducts)
        {
            return new UserFavoriteProductsListDto
            {
                UserId = userId,
                FavoriteProducts = favoriteProducts.Select(ProductDto.FromDomainModel).ToList() 
            };
        }
    }
}
