namespace Dal.Entities;

public class TrustedHardware : INamedEntity
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string HardwareId { get; set; } = null!;

    public virtual List<TrustedHardwareBenchmark> TrustedHardwareBenchmarks { get; } = new ();
}