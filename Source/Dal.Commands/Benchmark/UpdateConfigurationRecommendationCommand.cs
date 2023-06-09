using CleanModels.Commands.Base;
using Dal.Commands.Base;
using Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dal.Commands;

/// <inheritdoc />
public class UpdateConfigurationRecommendationCommand : DbCommandBase<List<ConfigurationRecommendation>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateOnlyNameUniqueEntityCommand{TEntity}"/> class.
    /// </summary>
    /// <param name="contextFactory">Context factory.</param>
    public UpdateConfigurationRecommendationCommand(IDbContextFactory<Context> contextFactory)
        : base(contextFactory)
    {
    }

    /// <inheritdoc/>
    protected override async Task<CommandResult> ExecuteCoreAsync(List<ConfigurationRecommendation> entitiesToUpdate)
    {
        var configurations = Context.Configurations.AsNoTracking();
        var existConfigNames = await configurations
            .Where(entity =>
                !entitiesToUpdate.Select(entityToUpdate => entityToUpdate.Id).Contains(entity.Id) &&
                entitiesToUpdate.Select(entityToUpdate => entityToUpdate.Name).Contains(entity.Name))
            .Select(entity => entity.Name)
            .ToListAsync();

        if (existConfigNames.Any())
        {
            if (entitiesToUpdate.Count == 1)
            {
                return GetFailedResult(@"Конфигурация с таким названием или уже существует.");
            }

            if (entitiesToUpdate.Count > 1)
            {
                string names = string.Join(',', existConfigNames);
                var errorMessage = @$"Конфигурации с таким названием уже существуют: {names}";

                return GetFailedResult(errorMessage);
            }
        }

        var entitiesToFetch = Context.ConfigurationRecommendations.AsNoTracking();
        var existEntityNames = await entitiesToFetch
            .Where(entity =>
                !entitiesToUpdate.Select(entityToUpdate => entityToUpdate.Id).Contains(entity.Id) &&
                (entitiesToUpdate.Select(entityToUpdate => entityToUpdate.Name).Contains(entity.Name) ||
                 (entitiesToUpdate.Select(entityToUpdate => entityToUpdate.VerificationResult)
                      .Contains(entity.VerificationResult) &&
                  entitiesToUpdate.Select(entityToUpdate => entityToUpdate.Configuration.Name)
                      .Contains(entity.Configuration.Name))))
            .Select(entity => entity.Name)
            .ToListAsync();

        if (existConfigNames.Any())
        {
            if (entitiesToUpdate.Count == 1)
            {
                return GetFailedResult(@"Рекомендация конфигурации с таким названием или " +
                                       @"названием конфигурации и результатом команды уже существует.");
            }

            if (entitiesToUpdate.Count > 1)
            {
                string names = string.Join(',', existEntityNames);
                string errorMessage = @"Рекомендации конфигураций с таким названием или " +
                                      @$"названием конфигурации и результатом команды уже существуют: {names}";

                return GetFailedResult(errorMessage);
            }
        }

        Context.UpdateRange(entitiesToUpdate);
        await Context.SaveChangesAsync();

        return GetSuccessfulResult();
    }
}