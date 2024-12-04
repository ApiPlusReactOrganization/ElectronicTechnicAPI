using Domain.Authentications.Users;
using Domain.CartItems;
using Domain.Products;

namespace Tests.Data;

public class CartItemsData
{
    public static CartItem MainCartItem(UserId userId, ProductId productId) =>
        CartItem.New(CartItemId.New(), userId, productId, 1);
}