namespace CleanModels.Benchmark;

public class Benchmark
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int ComputerQuantity { get; set; }

    public ConfigurationRecommendationsBenchmark? ConfigurationRecommendationsBenchmark { get; set; }

    public TrustedSoftwareBenchmark? TrustedSoftwareBenchmark { get; set; }

    public TrustedHardwareBenchmark? TrustedHardwareBenchmark { get; set; }
}