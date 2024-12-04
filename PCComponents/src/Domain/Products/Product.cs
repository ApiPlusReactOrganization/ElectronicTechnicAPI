using Domain.CartItems;
using Domain.Categories;
using Domain.Manufacturers;
using Domain.Products.PCComponents;

namespace Domain.Products
{
    public class Product
    {
        public ProductId Id { get; }
        public ComponentCharacteristic ComponentCharacteristic { get; private set; }
        public string Name { get; private set; }
        public decimal Price { get; private set; }
        public string Description { get; private set; }
        public int StockQuantity { get; private set; }
        public ManufacturerId ManufacturerId { get; private set; }
        public Manufacturer? Manufacturer { get; set; }
        public CategoryId CategoryId { get; private set; }
        public Category? Category { get; set; }
        public List<ProductImage> Images { get; private set; } = [];
        public List<CartItem> CartItems { get; private set; } = new();

        private Product(ProductId id, string name, decimal price, string description, int stockQuantity,
            ManufacturerId manufacturerId, CategoryId categoryId)
        {
            Id = id;
            Name = name;
            Price = price;
            Description = description;
            StockQuantity = stockQuantity;
            ManufacturerId = manufacturerId;
            CategoryId = categoryId;
        }

        public static Product New(ProductId id, string name, decimal price, string description, int stockQuantity,
            ManufacturerId manufacturerId, CategoryId categoryId, ComponentCharacteristic componentCharacteristic)
            => new(id, name, price, description, stockQuantity, manufacturerId, categoryId)
                { ComponentCharacteristic = componentCharacteristic };

        public void UpdateDetails(string name, decimal price, string description, int stockQuantity,
            CategoryId categoryId, ManufacturerId manufacturerId,
            ComponentCharacteristic componentCharacteristic)
        {
            Name = name;
            Price = price;
            Description = description;
            StockQuantity = stockQuantity;
            ComponentCharacteristic = componentCharacteristic;
            CategoryId = categoryId;
            ManufacturerId = manufacturerId;
        }
        
        public void UploadProductImages(List<ProductImage> images)
            => Images.AddRange(images);
        
        public void RemoveImage(ProductImageId productImageId)
        {
            var image = Images.FirstOrDefault(x => x.Id == productImageId);
            if (image != null)
            {
                Images.Remove(image);
            }
        }
        
        public void SetStockQuantity(int quantity)
         => StockQuantity = quantity;
    }
}