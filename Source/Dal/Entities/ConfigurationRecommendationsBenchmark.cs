namespace Dal.Entities;

public class ConfigurationRecommendationsBenchmark : INamedEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual List<Benchmark> Benchmarks { get; } = new ();

    public virtual List<ConfigurationRecommendation> ConfigurationRecommendations { get; } = new ();
}