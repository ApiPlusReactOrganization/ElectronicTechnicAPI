using Domain.Authentications.Users;
using Domain.CartItems;
using Domain.Orders;
using Domain.Products;

namespace Tests.Data;

public static class OrdersData
{
    public static Order MainOrder(UserId userId) =>
        Order.New(
            OrderId.New(),
            userId,
            StatusesConstants.Processing, // Статус замовлення
            "123 Main Street", // Адреса доставки
            new List<CartItem>
            {
               
            }
        );


    public static Order NewOrder(UserId userId) =>
        Order.New(
            OrderId.New(),
            userId,
            StatusesConstants.Processing,
            "456 Elm Street",
            new List<CartItem>
            {
                
            }
        );

   
    public static Order CustomOrder(UserId userId, List<CartItem> cartItems, string status, string deliveryAddress) =>
        Order.New(
            OrderId.New(),
            userId,
            status,
            deliveryAddress,
            cartItems
        );
}