using CleanModels.Benchmark;
using CsvHelper.Configuration;

namespace Client.Services.Mappers.Csv;

public sealed class TrustedSoftwareMapper : ClassMap<TrustedSoftware>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TrustedSoftwareMapper"/> class.
    /// </summary>
    public TrustedSoftwareMapper()
    {
        Map(software => software.Name).Name("Название");
        Map(software => software.Version).Name("Версия");
    }
}