using CleanModels.Queries.Base;
using Dal.Entities;
using Dal.Queries.Base;
using Microsoft.EntityFrameworkCore;

namespace Dal.Queries;

/// <inheritdoc />
public class ExistConfigurationRecommendationsQuery :
    DbQueryBase<List<ConfigurationRecommendation>, List<ConfigurationRecommendation>>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExistConfigurationRecommendationsQuery"/> class.
    /// </summary>
    /// <param name="contextFactory">Context factory.</param>
    public ExistConfigurationRecommendationsQuery(IDbContextFactory<Context> contextFactory)
        : base(contextFactory)
    {
    }

    /// <inheritdoc/>
    protected override async Task<QueryResult<List<ConfigurationRecommendation>>> ExecuteCoreAsync(
        List<ConfigurationRecommendation> recommendations)
    {
        var entitiesToFetch = Context.ConfigurationRecommendations.AsNoTracking();
        var existEntities = await entitiesToFetch
            .Where(entity =>
                recommendations.Select(entityToAdd => entityToAdd.Name).Contains(entity.Name) ||
                (recommendations.Select(entityToAdd => entityToAdd.VerificationResult)
                     .Contains(entity.VerificationResult) &&
                 recommendations.Select(entityToAdd => entityToAdd.Configuration.Name)
                     .Contains(entity.Configuration.Name)))
            .Select(entity =>
                new ConfigurationRecommendation
                {
                    Id = entity.Id,
                    Name = entity.Name,
                })
            .ToListAsync();

        return GetSuccessfulResult(existEntities);
    }
}