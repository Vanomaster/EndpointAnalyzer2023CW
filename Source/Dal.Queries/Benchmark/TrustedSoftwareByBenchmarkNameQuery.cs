using CleanModels.Queries.Base;
using Dal.Entities;
using Dal.Queries.Base;
using Microsoft.EntityFrameworkCore;

namespace Dal.Queries;

/// <inheritdoc />
public class TrustedSoftwareByBenchmarkNameQuery : DbQueryBase<string, List<TrustedSoftware>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TrustedSoftwareByBenchmarkNameQuery"/> class.
    /// </summary>
    /// <param name="contextFactory">Context factory.</param>
    public TrustedSoftwareByBenchmarkNameQuery(IDbContextFactory<Context> contextFactory)
        : base(contextFactory)
    {
    }

    /// <inheritdoc/>
    protected override async Task<QueryResult<List<TrustedSoftware>>> ExecuteCoreAsync(string benchmarkName)
    {
        var trustedSoftware = await Context.Benchmarks
            .AsNoTracking()
            .Where(benchmark => benchmark.Name == benchmarkName && benchmark.TrustedSoftwareBenchmark != null)
            .SelectMany(benchmark => benchmark.TrustedSoftwareBenchmark!.TrustedSoftware)
            .ToListAsync();

        return GetSuccessfulResult(trustedSoftware);
    }
}