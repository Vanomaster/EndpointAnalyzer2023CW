using CleanModels.Commands.Base;
using Dal.Commands.Base;
using Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dal.Commands;

/// <inheritdoc />
public class RemoveSoftCommand : DbCommandBase<IEnumerable<TrustedSoftware>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveEntitiesCommand{TEntity}"/> class.
    /// </summary>
    /// <param name="contextFactory">Context factory.</param>
    public RemoveSoftCommand(IDbContextFactory<Context> contextFactory)
        : base(contextFactory)
    {
    }

    /// <inheritdoc/>
    protected override async Task<CommandResult> ExecuteCoreAsync(IEnumerable<TrustedSoftware> entities)
    {
        var configRec = await Context.TrustedSoftware
            .AsNoTracking()
            .Where(e => e.Id == entities.FirstOrDefault().Id)
            .Include(x => x.TrustedSoftwareBenchmarks)
            .FirstOrDefaultAsync();

        var firstOrDefaultAsync = await Context.ConfigurationRecommendationBenchmarks
            .FirstOrDefaultAsync(e => e.Id == configRec.TrustedSoftwareBenchmarks[0].Id);

        Context.RemoveRange(entities);
        await Context.SaveChangesAsync();

        return GetSuccessfulResult();
    }
}