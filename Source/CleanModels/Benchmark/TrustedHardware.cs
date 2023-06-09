namespace CleanModels.Benchmark;

public class TrustedHardware
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string HardwareId { get; set; } = null!;

    public Guid? ParentId { get; set; }
}