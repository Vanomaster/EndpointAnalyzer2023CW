using Common.Extensions;
using Dal.Entities;
using Dal.Queries.Base;
using Microsoft.EntityFrameworkCore;

namespace Dal.Queries;

/// <inheritdoc />
public class AnalysisScheduleRecordQuery : PageQueryBase<AnalysisScheduleRecord>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AnalysisScheduleRecordQuery"/> class.
    /// </summary>
    /// <param name="contextFactory">Context factory.</param>
    public AnalysisScheduleRecordQuery(IDbContextFactory<Context> contextFactory)
        : base(contextFactory)
    {
    }

    /// <inheritdoc/>
    protected override IQueryable<AnalysisScheduleRecord> FilterBySearchWord(
        IQueryable<AnalysisScheduleRecord> source,
        string word)
    {
        return source.Where(entity => (entity.PcIp + " " + entity.BenchmarkName).ToLower().Contains(word.ToLower()));
    }

    /// <inheritdoc/>
    protected override IQueryable<AnalysisScheduleRecord> DefaultOrderBy(IQueryable<AnalysisScheduleRecord> source)
    {
        return source.OrderByDescending(entity => entity.Name);
    }

    // /// <inheritdoc/>
    // protected override object GetDefaultForOrderBy(AnalysisScheduleRecord entity)
    // {
    //     //return entity.Name;
    //     return entity.Recurrence.GetNextAnalysisDateTime();
    // }
}