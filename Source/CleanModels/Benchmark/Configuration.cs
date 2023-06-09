using MemoryPack;

namespace CleanModels.Benchmark;

[MemoryPackable]
public partial class Configuration
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }
}