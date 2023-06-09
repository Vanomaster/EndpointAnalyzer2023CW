using CleanModels.Commands.Base;
using Dal.Commands.Base;
using Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dal.Commands;

/// <inheritdoc />
public class AddConfigurationRecommendationCommand : DbCommandBase<List<ConfigurationRecommendation>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AddConfigurationRecommendationCommand"/> class.
    /// </summary>
    /// <param name="contextFactory">Context factory.</param>
    public AddConfigurationRecommendationCommand(IDbContextFactory<Context> contextFactory)
        : base(contextFactory)
    {
    }

    /// <inheritdoc/>
    protected override async Task<CommandResult> ExecuteCoreAsync(List<ConfigurationRecommendation> entitiesToAdd)
    {
        var parent = await Context.ConfigurationRecommendationBenchmarks.FirstOrDefaultAsync(b => b.Id == entitiesToAdd[0].ConfigurationRecommendationsBenchmarks[0].Id);
        entitiesToAdd.ForEach(e => e.ConfigurationRecommendationsBenchmarks.Clear());
        await Context.AddRangeAsync(entitiesToAdd);
        parent.ConfigurationRecommendations.AddRange(entitiesToAdd);
        await Context.SaveChangesAsync();

        return GetSuccessfulResult();
    }
}