using CleanModels.Queries.Base;
using Dal.Entities;
using Dal.Queries.Base;
using Microsoft.EntityFrameworkCore;

namespace Dal.Queries;

/// <inheritdoc />
public class AllConfigurationRecommendationsByBenchmarkNameQuery : DbQueryBase<string, List<ConfigurationRecommendation>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AllConfigurationRecommendationsByBenchmarkNameQuery"/> class.
    /// </summary>
    /// <param name="contextFactory">Context factory.</param>
    public AllConfigurationRecommendationsByBenchmarkNameQuery(IDbContextFactory<Context> contextFactory)
        : base(contextFactory)
    {
    }

    /// <inheritdoc/>
    protected override async Task<QueryResult<List<ConfigurationRecommendation>>> ExecuteCoreAsync(string benchmarkName)
    {
        var configurationRecommendations = await Context.Benchmarks
            .AsNoTracking()
            .Where(benchmark => benchmark.Name == benchmarkName &&
                                benchmark.ConfigurationRecommendationsBenchmark != null)
            .SelectMany(benchmark => benchmark.ConfigurationRecommendationsBenchmark!.ConfigurationRecommendations)
            .Include(e => e.Configuration)
            .ToListAsync();

        return GetSuccessfulResult(configurationRecommendations);
    }
}