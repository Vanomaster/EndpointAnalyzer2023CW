namespace Dal.Entities;

public class TrustedSoftware : INamedEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Version { get; set; } = null!;

    public virtual List<TrustedSoftwareBenchmark> TrustedSoftwareBenchmarks { get; } = new ();
}