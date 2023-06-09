namespace CleanModels.Benchmark;

public class TrustedSoftwareBenchmark
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public IEnumerable<TrustedSoftware> TrustedSoftware { get; set; }

    public Guid? ParentId { get; set; }
}