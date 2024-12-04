using Domain.Orders;

namespace Api.Dtos;

public record OrderDto(
    Guid? Id,
    Guid? UserId,
    decimal? TotalPrice,
    string Status,
    DateTime? CreatedAt,
    string DeliveryAddress,
    List<CartItemDto>? CartItems
)
{
    public static OrderDto FromDomainModel(Order order)
        => new(
            Id: order.Id.Value,
            UserId: order.UserId.Value,
            TotalPrice: order.TotalPrice,
            Status: order.StatusId,
            CreatedAt: order.CreatedAt,
            DeliveryAddress: order.DeliveryAddress,
            CartItems: order.Cart?.Count == 0 ? null : order.Cart?.Select(CartItemDto.FromDomainModel).ToList()
            );
}