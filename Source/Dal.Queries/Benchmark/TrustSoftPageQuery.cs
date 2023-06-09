using CleanModels;
using CleanModels.Queries.Base;
using Dal.Entities;
using Dal.Queries.Base;
using Microsoft.EntityFrameworkCore;

namespace Dal.Queries;

/// <inheritdoc />
public class TrustSoftPageQuery : DbQueryBase<PageModel, List<TrustedSoftware>>
{
    private const string Separator = " ";
    private const byte PageSize = 10;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigurationRecommendationsPageQuery"/> class.
    /// </summary>
    /// <param name="contextFactory">Context factory.</param>
    public TrustSoftPageQuery(IDbContextFactory<Context> contextFactory)
        : base(contextFactory)
    {
    }

    /// <inheritdoc/>
    protected override async Task<QueryResult<List<TrustedSoftware>>> ExecuteCoreAsync(PageModel pageModel)
    {
        var entitiesToFetch = Context.TrustedSoftwareBenchmarks.AsNoTracking();
        if (pageModel.ParentName is null)
        {
            return GetFailedResult(@"Родительский шаблон конфигураций не выбран.");
        }

        var entities = entitiesToFetch
            .Where(benchmark => benchmark.Name == pageModel.ParentName)
            .SelectMany(benchmark => benchmark.TrustedSoftware);

        if (pageModel.SearchPhrase is not null)
        {
            string[] searchWords = pageModel.SearchPhrase.Split(Separator);
            entities = entities.Where(entity =>
                searchWords.All(word =>
                    (entity.Name + " " + entity.Version).ToLower().Contains(word.ToLower())));
        }

        // if (pageModel.SortingOrderIsAscending is not null)
        // {
        //     // bool sortingOrderIsAscending = pageModel.SortingOrderIsAscending ?? false;
        //     // if (sortingOrderIsAscending)
        //     // {
        //     //     recommendations = recommendations
        //     //         .OrderBy(entity => GetEntityPropertyValueForOrderBy(entity, pageModel.SortingPropertyName));
        //     // }
        //     //
        //     // if (!sortingOrderIsAscending)
        //     // {
        //     //     recommendations = recommendations
        //     //         .OrderByDescending(
        //     //             entity => GetEntityPropertyValueForOrderBy(entity, pageModel.SortingPropertyName));
        //     // }
        // }

        int entitiesCountToSkip = (pageModel.PageNumber - 1) * PageSize;
        var configurationRecommendations = await entities
            .Skip(entitiesCountToSkip)
            .Take(PageSize)
            .ToListAsync();

        return GetSuccessfulResult(configurationRecommendations);
    }

    // private static object GetDefaultForOrderBy(ConfigurationRecommendation entity)
    // {
    //     return entity.Name;
    // }
    //
    // private object GetEntityPropertyValueForOrderBy(ConfigurationRecommendation entity, string? sortingPropertyName)
    // {
    //     if (sortingPropertyName == null)
    //     {
    //         return GetDefaultForOrderBy(entity);
    //     }
    //
    //     var prop = typeof(ConfigurationRecommendation).GetProperty(sortingPropertyName);
    //     object? propertyValue = prop.GetValue(entity, null);
    //     if (propertyValue == null)
    //     {
    //         return GetDefaultForOrderBy(entity);
    //     }
    //
    //     return propertyValue;
    // }
}