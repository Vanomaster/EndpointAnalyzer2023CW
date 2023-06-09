namespace CleanModels.Benchmark;

public class TrustedSoftware
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string Version { get; set; } = null!;

    public Guid? ParentId { get; set; }
}