using CleanModels.Queries.Base;
using Dal.Extensions;
using Dal.Queries.Base;
using Microsoft.EntityFrameworkCore;

namespace Dal.Queries;

/// <inheritdoc />
public class PcQuantityByBenchmarkNameQuery : DbQueryBase<string, int>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PcQuantityByBenchmarkNameQuery"/> class.
    /// </summary>
    /// <param name="contextFactory">Context factory.</param>
    public PcQuantityByBenchmarkNameQuery(IDbContextFactory<Context> contextFactory)
        : base(contextFactory)
    {
    }

    /// <inheritdoc/>
    protected override async Task<QueryResult<int>> ExecuteCoreAsync(string benchmarkName)
    {
        int pcQuantity = await Context.AnalysisScheduleRecords
            .AsNoTracking()
            .Where(record => record.BenchmarkName == benchmarkName)
            .UsingGroupDistinctBy(record => record.PcIp)
            .CountAsync();

        return GetSuccessfulResult(pcQuantity);
    }
}