namespace Dal.Entities;

public class TrustedSoftwareBenchmark : INamedEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual List<Benchmark> Benchmarks { get; } = new ();

    public virtual List<TrustedSoftware> TrustedSoftware { get; } = new ();
}