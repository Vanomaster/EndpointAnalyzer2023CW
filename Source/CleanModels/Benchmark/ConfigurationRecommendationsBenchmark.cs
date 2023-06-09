namespace CleanModels.Benchmark;

public class ConfigurationRecommendationsBenchmark
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public IEnumerable<ConfigurationRecommendation> ConfigurationRecommendations { get; set; }

    public Guid? ParentId { get; set; }
}