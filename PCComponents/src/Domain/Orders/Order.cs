using Domain.Authentications.Users;
using Domain.CartItems;

namespace Domain.Orders;

public class Order
{
    public OrderId Id { get; } 
    
    public UserId UserId { get; private set; } 
    
    public User? User { get; set; } 
    
    public List<CartItem>? Cart { get; private set; } = new();
    
    public decimal TotalPrice { get; private set; }
    
    public string Status { get; private set; } 
    
    public DateTime CreatedAt { get; private set; } 
    
    public string DeliveryAddress { get; private set; }

    private Order(OrderId id, UserId userId, string status, string deliveryAddress)
    {
        Id = id;
        UserId = userId;
        Status = status;
        CreatedAt = DateTime.UtcNow;
        DeliveryAddress = deliveryAddress;
    }
    
    public static Order New(OrderId id, UserId userId, string status, string deliveryAddress, List<CartItem> cart)
    {
        return new Order(id, userId, status, deliveryAddress)
        {
            TotalPrice = cart.Sum(x => x.Product!.Price * x.Quantity),
            Cart = cart
        };
    }
    
    public void UpdateStatus(string status)
    {
        Status = status;
    }
}