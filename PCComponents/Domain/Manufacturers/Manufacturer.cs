using Domain.Products;

namespace Domain.Manufacturers
{
    public class Manufacturer
    {
        public ManufacturerId Id { get; } // Унікальний ідентифікатор виробника
        public string Name { get; private set; } // Назва виробника
        
        public List<Product> Products { get; } = new();
        
        // Конструктор для Manufacturer
        private Manufacturer(ManufacturerId id, string name)
        {
            Id = id;
            Name = name;
        }

        // Метод для створення нового екземпляра Manufacturer
        public static Manufacturer New(ManufacturerId id, string name)
        {
            return new Manufacturer(id, name);
        }

        // Метод для оновлення назви виробника
        public void UpdateName(string name)
        {
            Name = name;
        }
    }
}
