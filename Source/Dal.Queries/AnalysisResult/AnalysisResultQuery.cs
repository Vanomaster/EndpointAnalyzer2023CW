using Dal.Entities;
using Dal.Queries.Base;
using Microsoft.EntityFrameworkCore;

namespace Dal.Queries;

/// <inheritdoc />
public class AnalysisResultQuery : PageQueryBase<AnalysisResult>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AnalysisResultQuery"/> class.
    /// </summary>
    /// <param name="contextFactory">Context factory.</param>
    public AnalysisResultQuery(IDbContextFactory<Context> contextFactory)
        : base(contextFactory)
    {
    }

    /// <inheritdoc/>
    protected override IQueryable<AnalysisResult> FilterBySearchWord(IQueryable<AnalysisResult> source, string word)
    {
        return source.Where(entity =>
            (entity.PcName + " " + entity.BenchmarkName + " " + entity.AnalyzerName + " " + entity.DateTime).ToLower()
            .Contains(word.ToLower()));
    }

    /// <inheritdoc/>
    protected override IQueryable<AnalysisResult> DefaultOrderBy(IQueryable<AnalysisResult> source)
    {
        return source.OrderByDescending(entity => entity.DateTime);
    }

    // /// <inheritdoc/>
    // protected override object GetDefaultForOrderBy(AnalysisResult entity)
    // {
    //     return entity.DateTime.ToString();
    // }
}