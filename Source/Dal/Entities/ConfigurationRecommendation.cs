namespace Dal.Entities;

public class ConfigurationRecommendation : INamedEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public Guid ConfigurationId { get; set; }

    public string VerificationCommand { get; set; }

    public string VerificationResult { get; set; }

    public virtual Configuration Configuration { get; set; } = null!;

    public virtual List<ConfigurationRecommendationsBenchmark> ConfigurationRecommendationsBenchmarks { get; } = new ();
}