using CleanModels.Commands.Base;
using Dal.Commands.Base;
using Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dal.Commands;

/// <inheritdoc />
public class UpdateConfigurationRecommendationsBenchmarkCommand : DbCommandBase<ConfigurationRecommendationsBenchmark>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateConfigurationRecommendationsBenchmarkCommand"/> class.
    /// </summary>
    /// <param name="contextFactory">Context factory.</param>
    public UpdateConfigurationRecommendationsBenchmarkCommand(IDbContextFactory<Context> contextFactory)
        : base(contextFactory)
    {
    }

    /// <inheritdoc/>
    protected override async Task<CommandResult> ExecuteCoreAsync(ConfigurationRecommendationsBenchmark entityToAdd)
    {
        var entitiesToFetch = Context.ConfigurationRecommendationBenchmarks.AsNoTracking();
        bool entityIsExisted = await entitiesToFetch.AnyAsync(entity => entity.Name == entityToAdd.Name);
        if (entityIsExisted)
        {
            return GetFailedResult(@"Шаблон с таким названием уже существует.");
        }

        var parent = await Context.Benchmarks.FirstOrDefaultAsync(b => b.Id == entityToAdd.Benchmarks[0].Id);
        entityToAdd.Benchmarks.Clear();
        //await Context.AddAsync(entityToAdd);
        parent.ConfigurationRecommendationsBenchmark = entityToAdd;
        Context.Entry(entityToAdd).State = EntityState.Unchanged;
        await Context.SaveChangesAsync();

        return GetSuccessfulResult();
    }
}