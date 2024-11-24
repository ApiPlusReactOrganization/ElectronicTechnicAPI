using Domain.Authentications.Users;
using Domain.Carts;

namespace Domain.Orders;

public class Order
{
    public OrderId Id { get; } 
    
    public UserId UserId { get; private set; } 
    
    public User? User { get; set; } 
    
    public CartId CartId { get; private set; } 
    
    public Cart? Cart { get; set; } 
    
    public decimal TotalPrice { get; private set; }
    
    public string Status { get; private set; } 
    
    public DateTime CreatedAt { get; private set; } 
    
    public string DeliveryAddress { get; private set; }

    private Order(OrderId id, UserId userId, CartId cartId, string status, string deliveryAddress)
    {
        Id = id;
        UserId = userId;
        CartId = cartId;
        Status = status;
        CreatedAt = DateTime.UtcNow;
        DeliveryAddress = deliveryAddress;
    }
    
    public static Order New(OrderId id, UserId userId, Cart cart, string status, string deliveryAddress)
    {
        return new Order(id, userId, cart.Id, status, deliveryAddress)
        {
            Cart = cart,
            TotalPrice = cart.Items.Sum(x => x.Product!.Price * x.Quantity)
        };
    }
    
    public void UpdateStatus(string status)
    {
        Status = status;
    }
}