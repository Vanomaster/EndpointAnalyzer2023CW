using CleanModels;
using CleanModels.Queries.Base;
using Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dal.Queries.Base;

/// <inheritdoc />
public abstract class PageQueryBase<TEntity> : DbQueryBase<PageModel, List<TEntity>>
    where TEntity : class, IEntity
{
    private const string Separator = " ";
    private const byte PageSize = 10;

    /// <summary>
    /// Initializes a new instance of the <see cref="PageQueryBase{TEntity}"/> class.
    /// </summary>
    /// <param name="contextFactory">Context factory.</param>
    protected PageQueryBase(IDbContextFactory<Context> contextFactory)
        : base(contextFactory)
    {
    }

    /// <inheritdoc/>
    protected override async Task<QueryResult<List<TEntity>>> ExecuteCoreAsync(PageModel pageModel)
    {
        var entitiesToFetch = Context.Set<TEntity>().AsNoTracking();
        if (pageModel.SearchPhrase is not null)
        {
            string[] searchWords = pageModel.SearchPhrase.Split(Separator);
            foreach (string word in searchWords)
            {
                entitiesToFetch = FilterBySearchWord(entitiesToFetch, word);
            }
        }

        entitiesToFetch = DefaultOrderBy(entitiesToFetch);
        // if (pageModel.SortingOrderIsAscending is not null)
        // {
        //     // bool sortingOrderIsAscending = pageModel.SortingOrderIsAscending ?? false;
        //     // if (sortingOrderIsAscending)
        //     // {
        //     //     entitiesToFetch = entitiesToFetch
        //     //         .OrderBy(entity => GetEntityPropertyValueForOrderBy(entity, pageModel.SortingPropertyName));
        //     // }
        //     //
        //     // if (!sortingOrderIsAscending)
        //     // {
        //     //     entitiesToFetch = entitiesToFetch
        //     //         .OrderByDescending(
        //     //             entity => GetEntityPropertyValueForOrderBy(entity, pageModel.SortingPropertyName));
        //     // }
        // }

        int entitiesCountToSkip = (pageModel.PageNumber - 1) * PageSize;
        var entities = await entitiesToFetch
            .Skip(entitiesCountToSkip)
            .Take(PageSize)
            .ToListAsync();

        return GetSuccessfulResult(entities);
    }

    protected abstract IQueryable<TEntity> FilterBySearchWord(IQueryable<TEntity> source, string word);

    protected abstract IQueryable<TEntity> DefaultOrderBy(IQueryable<TEntity> source);

    // private object GetEntityPropertyValueForOrderBy(TEntity entity, string? sortingPropertyName)
    // {
    //     if (sortingPropertyName == null)
    //     {
    //         return GetDefaultForOrderBy(entity);
    //     }
    //
    //     object? propertyValue = entity.GetPropertyValue(sortingPropertyName.GetTypePropertyByName<TEntity>()); // TODO if will not working, extract separate classes for each entity and inject here
    //     if (propertyValue == null)
    //     {
    //         return GetDefaultForOrderBy(entity);
    //     }
    //
    //     return propertyValue;
    // }
}