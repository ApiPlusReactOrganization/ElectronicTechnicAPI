using Domain.Authentications.Users;
using Domain.Carts;

namespace Domain.Orders;

public class Order
{
    public Guid Id { get; set; } 
    
    public UserId UserId { get; set; } 
    public User User { get; set; } 
    
    public Guid CartId { get; set; } 
    public Cart Cart { get; set; } 
    
    public decimal TotalPrice { get; set; } 
    
    public string Status { get; set; } 
    
    public DateTime CreatedAt { get; set; } 
    
    public string? DeliveryAddress { get; set; } 
}