using MemoryPack;

namespace CleanModels.Software;

[MemoryPackable]
public partial class Software
{
    public string Name { get; set; }

    public string Version { get; set; }

    public string Description { get; set; }
}