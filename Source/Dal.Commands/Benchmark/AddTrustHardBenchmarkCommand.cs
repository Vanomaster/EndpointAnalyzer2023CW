using CleanModels.Commands.Base;
using Dal.Commands.Base;
using Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dal.Commands;

/// <inheritdoc />
public class AddTrustHardBenchmarkCommand : DbCommandBase<TrustedHardwareBenchmark>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AddConfigurationRecommendationsBenchmarkCommand"/> class.
    /// </summary>
    /// <param name="contextFactory">Context factory.</param>
    public AddTrustHardBenchmarkCommand(IDbContextFactory<Context> contextFactory)
        : base(contextFactory)
    {
    }

    /// <inheritdoc/>
    protected override async Task<CommandResult> ExecuteCoreAsync(TrustedHardwareBenchmark entityToAdd)
    {
        var entitiesToFetch = Context.TrustedHardwareBenchmarks.AsNoTracking();
        bool entityIsExisted = await entitiesToFetch.AnyAsync(entity => entity.Name == entityToAdd.Name);
        if (entityIsExisted)
        {
            return GetFailedResult(@"Шаблон с таким названием уже существует.");
        }

        var parent = await Context.Benchmarks.FirstOrDefaultAsync(b => b.Id == entityToAdd.Benchmarks[0].Id);
        entityToAdd.Benchmarks.Clear();
        await Context.AddAsync(entityToAdd);
        parent.TrustedHardwareBenchmark = entityToAdd;
        await Context.SaveChangesAsync();

        return GetSuccessfulResult();
    }
}