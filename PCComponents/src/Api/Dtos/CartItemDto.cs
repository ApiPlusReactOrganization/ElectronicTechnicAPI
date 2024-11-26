using Api.Dtos.Products;
using Domain.CartItems;
using Domain.Products;
using Domain.Products.PCComponents;

namespace Api.Dtos;

public record CartItemDto(
    Guid? Id,
    int Quantity,
    Guid CartId,
    /*
    CartDto? Cart,
    */
    Guid ProductId,
    ProductDto? Product
)
{
    public static CartItemDto FromDomainModel(CartItem cartItem)
    {
        return new CartItemDto(
            Id: cartItem.Id.Value,
            Quantity: cartItem.Quantity,
            CartId: cartItem.CartId.Value,
            /*
            Cart: cartItem.Cart == null ? null : CartDto.FromDomainModel(cartItem.Cart),
            */
            ProductId: cartItem.ProductId.Value,
            Product: cartItem.Product == null ? null : ProductDto.FromDomainModel(cartItem.Product)
        );
    }
}