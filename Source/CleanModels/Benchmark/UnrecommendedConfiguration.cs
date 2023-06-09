using MemoryPack;

namespace CleanModels.Benchmark;

[MemoryPackable]
public partial class UnrecommendedConfiguration
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string VerificationCommand { get; set; }

    public string ExpectedVerificationResult { get; set; }

    public string ActualVerificationResult { get; set; }

    public Configuration Configuration { get; set; } = null!;
}