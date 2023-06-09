using CleanModels.Queries.Base;
using Dal.Entities;
using Dal.Queries.Base;
using Microsoft.EntityFrameworkCore;

namespace Dal.Queries;

/// <inheritdoc />
public class TrustedHardwareByBenchmarkNameQuery : DbQueryBase<string, List<TrustedHardware>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TrustedHardwareByBenchmarkNameQuery"/> class.
    /// </summary>
    /// <param name="contextFactory">Context factory.</param>
    public TrustedHardwareByBenchmarkNameQuery(IDbContextFactory<Context> contextFactory)
        : base(contextFactory)
    {
    }

    /// <inheritdoc/>
    protected override async Task<QueryResult<List<TrustedHardware>>> ExecuteCoreAsync(string benchmarkName)
    {
        var trustedSoftware = await Context.Benchmarks
            .AsNoTracking()
            .Where(benchmark => benchmark.Name == benchmarkName && benchmark.TrustedHardwareBenchmark != null)
            .SelectMany(benchmark => benchmark.TrustedHardwareBenchmark!.TrustedHardware)
            .ToListAsync();

        return GetSuccessfulResult(trustedSoftware);
    }
}