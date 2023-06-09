using CleanModels.Queries.Base;
using Dal.Queries.Base;
using Microsoft.EntityFrameworkCore;

namespace Dal.Queries;

/// <inheritdoc />
public class AnalysisScheduleRecordByPcIpAnyQuery : DbQueryBase<string, bool>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AnalysisScheduleRecordQuery"/> class.
    /// </summary>
    /// <param name="contextFactory">Context factory.</param>
    public AnalysisScheduleRecordByPcIpAnyQuery(IDbContextFactory<Context> contextFactory)
        : base(contextFactory)
    {
    }

    /// <inheritdoc/>
    protected override async Task<QueryResult<bool>> ExecuteCoreAsync(string pcIp)
    {
        bool analysisScheduleRecords = await Context.AnalysisScheduleRecords
            .AsNoTracking()
            .AnyAsync(entity => entity.PcIp == pcIp);

        return GetSuccessfulResult(analysisScheduleRecords);
    }
}