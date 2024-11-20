namespace Domain.Products.PCComponents;

public static class PCComponentsNames
{
    public const string CPU = "Processor";
    public const string Case = "Computer case";
    public const string GPU = "Graphics Card";
    public const string RAM = "RAM";
    public const string PSU = "Power Supply Unit";
    public const string Motherboard = "Motherboard";
    public const string Cooler = "Cooler";
    public const string HDD = "HDD";
    public const string SSD = "SSD";
    
    public static readonly List<string> ListOfComponents = new()
    {
        CPU,
        Case,
        GPU,
        RAM,
        PSU,
        Motherboard,
        Cooler,
        HDD,
        SSD
    };
    
    public static void UpdateComponentList(string oldName, string newName)
    {
        var index = ListOfComponents.IndexOf(oldName);
        if (index >= 0)
        {
            ListOfComponents[index] = newName;
        }
    }
}