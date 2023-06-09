using MemoryPack;

namespace CleanModels.Software;

[MemoryPackable]
public partial class UpgradableSoftware
{
    public string Name { get; set; }

    public string CurrentVersion { get; set; }

    public string NewVersion { get; set; }
}