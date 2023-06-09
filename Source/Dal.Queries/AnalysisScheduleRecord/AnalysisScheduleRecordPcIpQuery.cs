using CleanModels.Queries.Base;
using Dal.Queries.Base;
using Microsoft.EntityFrameworkCore;

namespace Dal.Queries;

/// <inheritdoc />
public class AnalysisScheduleRecordPcIpQuery : DbQueryBase<List<string>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AnalysisScheduleRecordQuery"/> class.
    /// </summary>
    /// <param name="contextFactory">Context factory.</param>
    public AnalysisScheduleRecordPcIpQuery(IDbContextFactory<Context> contextFactory)
        : base(contextFactory)
    {
    }

    /// <inheritdoc/>
    protected override async Task<QueryResult<List<string>>> ExecuteCoreAsync()
    {
        var pcNames = await Context.AnalysisScheduleRecords
            .AsNoTracking()
            .Select(record => record.PcIp)
            .ToListAsync();

        return GetSuccessfulResult(pcNames);
    }
}