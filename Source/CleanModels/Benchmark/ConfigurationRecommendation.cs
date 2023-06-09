using MemoryPack;

namespace CleanModels.Benchmark;

[MemoryPackable]
public partial class ConfigurationRecommendation
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string VerificationCommand { get; set; }

    public string VerificationResult { get; set; }

    public Configuration Configuration { get; set; } = null!;

    public Guid? ParentId { get; set; }
}