using Domain.Categories;
using Domain.Products;

namespace Domain.Manufacturers
{
    public class Manufacturer
    {
        public ManufacturerId Id { get; }
        public string Name { get; private set; }
        public List<Category> Categories { get; private set; } = new();
        
        private Manufacturer(ManufacturerId id, string name)
        {
            Id = id;
            Name = name;
        }

        public static Manufacturer New(ManufacturerId id, string name)
        {
            return new Manufacturer(id, name);
        }

        public void UpdateName(string name)
        {
            Name = name;
        }

        public void SetCategories(List<Category> categories)
        => Categories = categories;
    }
}
