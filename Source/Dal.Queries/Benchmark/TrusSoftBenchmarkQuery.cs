using CleanModels.Queries.Base;
using Dal.Entities;
using Dal.Queries.Base;
using Microsoft.EntityFrameworkCore;

namespace Dal.Queries;

/// <inheritdoc />
public class TrusSoftBenchmarkQuery : DbQueryBase<string, TrustedSoftwareBenchmark?>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationRecommendationsBenchmarkQuery"/> class.
    /// </summary>
    /// <param name="contextFactory">Context factory.</param>
    public TrusSoftBenchmarkQuery(IDbContextFactory<Context> contextFactory)
        : base(contextFactory)
    {
    }

    /// <inheritdoc/>
    protected override async Task<QueryResult<TrustedSoftwareBenchmark?>> ExecuteCoreAsync(
        string benchmarkName)
    {
        var entitiesToFetch = Context.Benchmarks.AsNoTracking();
        var entities = entitiesToFetch
            .Where(benchmark =>
                benchmark.Name == benchmarkName && benchmark.TrustedSoftwareBenchmarkId != null)
            .Select(benchmark => benchmark.TrustedSoftwareBenchmark);

        var benchmark = await entities.FirstOrDefaultAsync();
        // if (benchmark == default)
        // {
        //     return GetFailedResult(@"Шаблон конфигураций не найден.");
        // }

        return GetSuccessfulResult(benchmark);
    }
}