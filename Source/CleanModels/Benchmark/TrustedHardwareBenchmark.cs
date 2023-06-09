namespace CleanModels.Benchmark;

public class TrustedHardwareBenchmark
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public IEnumerable<TrustedHardware> TrustedHardware { get; set; }

    public Guid? ParentId { get; set; }
}