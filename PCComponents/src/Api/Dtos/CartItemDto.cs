using Api.Dtos.Products;
using Domain.CartItems;
using Domain.Products;
using Domain.Products.PCComponents;

namespace Api.Dtos;

public record CartItemDto(
    Guid? Id,
    Guid? UserId,
    int Quantity,
    Guid? ProductId,
    bool IsFinished,
    ProductDto? Product)
{
    public static CartItemDto FromDomainModel(CartItem cartItem)
        => new(
            Id: cartItem.Id.Value,
            UserId: cartItem.UserId.Value,
            Quantity: cartItem.Quantity,
            ProductId: cartItem.ProductId.Value,
            IsFinished: cartItem.IsFinished,
            Product: cartItem.Product == null ? null : ProductDto.FromDomainModel(cartItem.Product)
        );
}