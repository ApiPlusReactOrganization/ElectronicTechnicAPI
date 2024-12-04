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
    
    public DateTime CreatedAt { get; private set; } 
    
    public string DeliveryAddress { get; private set; }
    
    public Status? Status { get; set; }
    public string StatusId { get; private set; }

    private Order(OrderId id, UserId userId, string statusId, string deliveryAddress)
    {
        Id = id;
        UserId = userId;
        StatusId = statusId;
        CreatedAt = DateTime.UtcNow;
        DeliveryAddress = deliveryAddress;
    }
    
    public static Order New(OrderId id, UserId userId, string statusId, string deliveryAddress, List<CartItem> cart)
    {
        return new Order(id, userId, statusId, deliveryAddress)
        {
            TotalPrice = cart.Sum(x => x.Product!.Price * x.Quantity),
            Cart = cart
        };
    }
    
    public void UpdateStatus(string statusId)
    {
        StatusId = statusId;
    }
}