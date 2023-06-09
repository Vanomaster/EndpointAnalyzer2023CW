using CleanModels.Queries.Base;
using Dal.Entities;
using Dal.Queries.Base;
using Microsoft.EntityFrameworkCore;

namespace Dal.Queries;

/// <inheritdoc />
public class ConfigurationRecommendationsBenchmarkQuery : DbQueryBase<string, ConfigurationRecommendationsBenchmark?>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationRecommendationsBenchmarkQuery"/> class.
    /// </summary>
    /// <param name="contextFactory">Context factory.</param>
    public ConfigurationRecommendationsBenchmarkQuery(IDbContextFactory<Context> contextFactory)
        : base(contextFactory)
    {
    }

    /// <inheritdoc/>
    protected override async Task<QueryResult<ConfigurationRecommendationsBenchmark?>> ExecuteCoreAsync(
        string benchmarkName)
    {
        var entitiesToFetch = Context.Benchmarks.AsNoTracking();
        var entities = entitiesToFetch
            .Where(benchmark =>
                benchmark.Name == benchmarkName && benchmark.ConfigurationRecommendationsBenchmark != null)
            .Select(benchmark => benchmark.ConfigurationRecommendationsBenchmark);

        var benchmark = await entities.FirstOrDefaultAsync();
        // if (benchmark == default)
        // {
        //     return GetFailedResult(@"Шаблон конфигураций не найден.");
        // }

        return GetSuccessfulResult(benchmark);
    }
}