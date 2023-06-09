using MemoryPack;

namespace CleanModels.Hardware;

[MemoryPackable]
public partial class UnknownHardware
{
    public string HardwareId { get; set; }
}