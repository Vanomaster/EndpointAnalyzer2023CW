using CleanModels.Commands.Base;
using Dal.Commands.Base;
using Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dal.Commands;

/// <inheritdoc />
public class RemoveHardCommand : DbCommandBase<IEnumerable<TrustedHardware>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveEntitiesCommand{TEntity}"/> class.
    /// </summary>
    /// <param name="contextFactory">Context factory.</param>
    public RemoveHardCommand(IDbContextFactory<Context> contextFactory)
        : base(contextFactory)
    {
    }

    /// <inheritdoc/>
    protected override async Task<CommandResult> ExecuteCoreAsync(IEnumerable<TrustedHardware> entities)
    {
        var configRec = await Context.TrustedHardware
            .AsNoTracking()
            .Where(e => e.Id == entities.FirstOrDefault().Id)
            .Include(x => x.TrustedHardwareBenchmarks)
            .FirstOrDefaultAsync();

        var firstOrDefaultAsync = await Context.ConfigurationRecommendationBenchmarks
            .FirstOrDefaultAsync(e => e.Id == configRec.TrustedHardwareBenchmarks[0].Id);

        Context.RemoveRange(entities);
        await Context.SaveChangesAsync();

        return GetSuccessfulResult();
    }
}