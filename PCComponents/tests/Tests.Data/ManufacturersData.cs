using Domain.Manufacturers;

namespace Tests.Data;

public static class ManufacturersData
{
    public static Manufacturer MainManufacturer => Manufacturer.New(
        ManufacturerId.New(), "Main 123 Test Manufacturer");
}