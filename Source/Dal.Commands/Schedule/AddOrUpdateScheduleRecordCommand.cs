using CleanModels.Commands.Base;
using Dal.Commands.Base;
using Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dal.Commands.Schedule;

/// <inheritdoc />
public class AddOrUpdateScheduleRecordCommand : DbCommandBase<AnalysisScheduleRecord>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AddOrUpdateScheduleRecordCommand"/> class.
    /// </summary>
    /// <param name="contextFactory">Context factory.</param>
    /// <param name="equalityComparer">Equality comparer.</param>
    public AddOrUpdateScheduleRecordCommand(
        IDbContextFactory<Context> contextFactory,
        IEqualityComparer<AnalysisScheduleRecord> equalityComparer)
        : base(contextFactory)
    {
        EqualityComparer = equalityComparer;
    }

    private IEqualityComparer<AnalysisScheduleRecord> EqualityComparer { get; }

    /// <inheritdoc/>
    protected override async Task<CommandResult> ExecuteCoreAsync(AnalysisScheduleRecord analysisScheduleRecord)
    {
        var entitiesToFetch = Context.AnalysisScheduleRecords.AsNoTracking();
        bool analysisScheduleRecordIsExisted = await entitiesToFetch
            .AnyAsync(entity => EqualityComparer.Equals(entity, analysisScheduleRecord));

        if (analysisScheduleRecordIsExisted)
        {
            return GetFailedResult(@"Запись в планировщике с такими параметрами или названием уже существует.");
        }

        Context.Update(analysisScheduleRecord);
        await Context.SaveChangesAsync();

        return GetSuccessfulResult();
    }
}