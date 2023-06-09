namespace Dal.Entities;

public class Benchmark : INamedEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public Guid? ConfigurationRecommendationsBenchmarkId { get; set; }

    public Guid? TrustedSoftwareBenchmarkId { get; set; }

    public Guid? TrustedHardwareBenchmarkId { get; set; }

    public virtual ConfigurationRecommendationsBenchmark? ConfigurationRecommendationsBenchmark { get; set; }

    public virtual TrustedSoftwareBenchmark? TrustedSoftwareBenchmark { get; set; }

    public virtual TrustedHardwareBenchmark? TrustedHardwareBenchmark { get; set; }
}