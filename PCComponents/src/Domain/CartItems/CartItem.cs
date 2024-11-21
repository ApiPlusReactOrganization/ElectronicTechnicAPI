using Domain.Carts;
using Domain.Products;

namespace Domain.CartItems;

public class CartItem
{
    public Guid Id { get; set; } 
    
    public Guid CartId { get; set; } 
    public Cart Cart { get; set; } 

    public ProductId ProductId { get; set; } 
    public Product Product { get; set; } 

    public int Quantity { get; set; } 
}