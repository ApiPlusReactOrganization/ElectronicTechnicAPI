using Domain.Manufacturers;

namespace Domain.Categories;

public class Category
{
    public CategoryId Id { get; set; }
    public string Name { get; set; }
    public List<Manufacturer> Manufacturers { get; }
    
    private Category(CategoryId id, string name)
    {
        Id = id;
        Name = name;
    }
    
    public static Category New(CategoryId id, string name)
        => new(id, name);

    public void UpdateName(string name)
    {
        Name = name;
    }
}