using Domain.Authentications.Users;
using Domain.CartItems;
using Domain.Orders;

namespace Domain.Carts;

public class Cart
{
    public CartId Id { get; }

    public UserId UserId { get; private set; }
    
    public User? User { get; set; }

    public List<CartItem> Items { get; private set; } = new();
    
    public OrderId OrderId { get; private set; } 
    
    public Order? Order { get; set; } 

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