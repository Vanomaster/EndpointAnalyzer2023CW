using CleanModels.Commands.Base;
using Dal.Commands.Base;
using Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dal.Commands;

/// <inheritdoc />
public class RemoveConfigCommand : DbCommandBase<IEnumerable<ConfigurationRecommendation>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveEntitiesCommand{TEntity}"/> class.
    /// </summary>
    /// <param name="contextFactory">Context factory.</param>
    public RemoveConfigCommand(IDbContextFactory<Context> contextFactory)
        : base(contextFactory)
    {
    }

    /// <inheritdoc/>
    protected override async Task<CommandResult> ExecuteCoreAsync(IEnumerable<ConfigurationRecommendation> entities)
    {
        var configRec = await Context.ConfigurationRecommendations
            .AsNoTracking()
            .Where(e => e.Id == entities.FirstOrDefault().Id)
            .Include(x => x.ConfigurationRecommendationsBenchmarks)
            .FirstOrDefaultAsync();

        var firstOrDefaultAsync = await Context.ConfigurationRecommendationBenchmarks
            .AsNoTracking()
            .Include(x => x.ConfigurationRecommendations)
            .FirstOrDefaultAsync(e => e.Id == configRec.ConfigurationRecommendationsBenchmarks[0].Id);
        firstOrDefaultAsync.ConfigurationRecommendations.Remove(configRec);

        var configurations = await Context.Configurations
            .AsNoTracking()
            .Where(e => entities.Select(x => x.Configuration.Id).Contains(e.Id))
            .FirstOrDefaultAsync();
        // foreach (var e in entities)
        // {
        //     //e.ConfigurationId = e.Configuration.Id;
        //     //e.Configuration = null;
        // }

        //Context.RemoveRange(configurations);
        Context.Update(firstOrDefaultAsync);
        Context.RemoveRange(entities);
        await Context.SaveChangesAsync();

        return GetSuccessfulResult();
    }
}