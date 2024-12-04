using Value;

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
    //
    // public static void UpdateComponentList(string oldName, string newName)
    // {
    //     var index = ListOfComponents.IndexOf(oldName);
    //     if (index >= 0)
    //     {
    //         ListOfComponents[index] = newName;
    //     }
    // }
}

public class SelectionCategoryType(string Value) : ValueObject
{
    public static SelectionCategoryType From(string value)
    {
        var categoryType = new SelectionCategoryType(value);

        if (!SupportedSSelectionCategoryTypes.Contains(categoryType))
        {
            throw new ArgumentException($"Invalid PCComponents name: {value}");
        }
        
        return categoryType;
    }

    public static SelectionCategoryType CPU => new(PCComponentsNames.CPU);
    public static SelectionCategoryType Case => new(PCComponentsNames.Case);
    public static SelectionCategoryType GPU => new(PCComponentsNames.GPU);
    public static SelectionCategoryType RAM => new(PCComponentsNames.RAM);
    public static SelectionCategoryType PSU => new(PCComponentsNames.PSU);
    public static SelectionCategoryType Motherboard => new(PCComponentsNames.Motherboard);
    public static SelectionCategoryType Cooler => new(PCComponentsNames.Cooler);
    public static SelectionCategoryType HDD => new(PCComponentsNames.HDD);
    public static SelectionCategoryType SSD => new(PCComponentsNames.SSD);

    public static implicit operator string(SelectionCategoryType value)
    {
        return value.ToString();
    }

    public static explicit operator SelectionCategoryType(string value)
    {
        return From(value);
    }

    public override string ToString()
    {
        return Value;
    }

    private static IEnumerable<SelectionCategoryType> SupportedSSelectionCategoryTypes
    {
        get
        {
            yield return CPU;
            yield return Case;
            yield return GPU;
            yield return RAM;
            yield return PSU;
            yield return Motherboard;
            yield return Cooler;
            yield return HDD;
            yield return SSD;
        }
    }
}