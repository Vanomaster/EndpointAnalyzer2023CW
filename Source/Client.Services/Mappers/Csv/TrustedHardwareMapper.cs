using CleanModels.Benchmark;
using CsvHelper.Configuration;

namespace Client.Services.Mappers.Csv;

public sealed class TrustedHardwareMapper : ClassMap<TrustedHardware>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TrustedHardwareMapper"/> class.
    /// </summary>
    public TrustedHardwareMapper()
    {
        Map(hardware => hardware.Name).Name("Название");
        Map(hardware => hardware.HardwareId).Name("Идентификатор");
    }
}