using Domain.FormFactors;
using Domain.Sockets;

namespace Domain.ComponentCharacteristics
{
    public class ComponentCharacteristic
    {
        public Case? Case { get; set; }
        public CPU? Cpu { get; set; }
        public GPU? Gpu { get; set; }
        public Motherboard? Motherboard { get; set; }
        public PSU? PSU { get; private set; }

        public static ComponentCharacteristic NewCase(Case someCase)
            => new() { Case = someCase };

        public static ComponentCharacteristic NewCpu(CPU cpu)
            => new() { Cpu = cpu };

        public static ComponentCharacteristic NewGpu(GPU gpu)
            => new() { Gpu = gpu };

        public static ComponentCharacteristic NewMotherboard(Motherboard motherboard)
            => new() { Motherboard = motherboard };

        public static ComponentCharacteristic NewPsu(PSU psu)
            => new() { PSU = psu };
    }
}

public record Case
{
    public required int NumberOfFans { get; init; }
    public required string CoolingSystem { get; init; }
    public required FormFactor FormFactor { get; init; }
}

public record CPU
{
    public required string Model { get; init; }
    public required int Cores { get; init; }
    public required int Threads { get; init; }
    public required decimal BaseClock { get; init; } // GHz
    public required decimal BoostClock { get; init; } // GHz
    public required Socket Socket { get; init; }
}

public record GPU
{
    public required string Model { get; init; }
    public required int MemorySize { get; init; } // GB
    public required string MemoryType { get; init; }
    public required int CoreClock { get; init; } // MHz
    public required int BoostClock { get; init; } // MHz
    public required FormFactor FormFactor { get; init; }
}

public record Motherboard
{
    public required Socket Socket { get; init; }
    public required FormFactor FormFactor { get; init; }
    public required string RAMDescription { get; init; }
    public required string NetworkDescription { get; init; }
    public required string PowerDescription { get; init; }
    public required string AudioDescription { get; init; }
    public required string ExternalConnectorsDescription { get; init; }
}

public record PSU
{
    public required string PowerCapacity { get; init; }
    public required string InputVoltageRange { get; init; }
    public required string FanTypeAndSize { get; init; }
    public required string Protections { get; init; }
    public required string Connectors { get; init; }
}