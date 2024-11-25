using Domain.Authentications.Users;
using Domain.CartItems;
using Domain.Orders;

namespace Domain.Carts;

public class Cart
{
    public CartId Id { get; }

    public User? User { get; }
    
    public UserId UserId { get; }
    
    public List<CartItem> Items { get; private set; } = new();

    public List<Order> Orders { get; private set; } = new();

    private Cart(CartId id, UserId userId)
    {
        Id = id;
        UserId = userId;
    }
    
    public static Cart New(CartId id, UserId userId) 
        => new(id, userId);

    /*public void UpdateName(string name)
    {
        Name = name;
    }*/
}