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
        public PSU? Psu { get; set; }
        public RAM? Ram { get; set; }
        public Cooler? Cooler { get; set; }
        public HDD? Hdd { get; set; }
        public SDD? Sdd { get; set; }

        public static ComponentCharacteristic NewCase(Case someCase)
            => new() { Case = someCase };

        public static ComponentCharacteristic NewCpu(CPU cpu)
            => new() { Cpu = cpu };

        public static ComponentCharacteristic NewGpu(GPU gpu)
            => new() { Gpu = gpu };

        public static ComponentCharacteristic NewMotherboard(Motherboard motherboard)
            => new() { Motherboard = motherboard };

        public static ComponentCharacteristic NewPsu(PSU psu)
            => new() { Psu = psu };

        public static ComponentCharacteristic NewRam(RAM ram)
            => new() { Ram = ram };

        public static ComponentCharacteristic NewCooler(Cooler cooler)
            => new() { Cooler = cooler };

        public static ComponentCharacteristic NewHdd(HDD hdd)
            => new() { Hdd = hdd };

        public static ComponentCharacteristic NewSDD(SDD sdd)
            => new() { Sdd = sdd };
    }
}

public record Case
{
    public required int NumberOfFans { get; init; }
    public required string CoolingSystem { get; init; }
    public required FormFactor FormFactor { get; set; }
}

public record CPU
{
    public required string Model { get; init; }
    public required int Cores { get; init; }
    public required int Threads { get; init; }
    public required decimal BaseClock { get; init; } // GHz
    public required decimal BoostClock { get; init; } // GHz
    public required Socket Socket { get; set; }
}

public record GPU
{
    public required string Model { get; init; }
    public required int MemorySize { get; init; } // GB
    public required string MemoryType { get; init; }
    public required int CoreClock { get; init; } // MHz
    public required int BoostClock { get; init; } // MHz
    public required FormFactor FormFactor { get; set; }
}

public record Motherboard
{
    public required Socket Socket { get; set; }
    public required FormFactor FormFactor { get; set; }
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

public record RAM
{
    public required int MemoryAmount { get; init; }
    public required int MemorySpeed { get; init; }

    // maybe make MemoryType as model
    public required string MemoryType { get; init; }
    public required FormFactor FormFactor { get; init; }

    public required float Voltage { get; init; }

    //MemoryBandwidth - пропускна здатність 
    public required float MemoryBandwidth { get; init; }
}

public record Cooler
{
    //maybe make Material as a model as 
    public required string Material { get; init; }
    public required int Fanspeed { get; init; }
    public required int FanAmount { get; init; }
    public required int Voltage { get; init; }
    public required int MaxTDP { get; init; }
    public required List<Socket> Sockets { get; init; }
    public required string FanSupply { get; init; }
}

public record HDD
{
    public required int MemoryAmount { get; init; }
    public required FormFactor FormFactor { get; init; }
    public required float Voltage { get; init; }
    public required int ReadSpeed { get; init; }
    public required int WriteSpeed { get; init; }
}

public record SDD
{
    public required int MemoryAmount { get; init; }
    public required FormFactor FormFactor { get; init; }
    public required int ReadSpeed { get; init; }
    public required int WriteSpeed { get; init; }
    public required int MaxTBW { get; init; }
}