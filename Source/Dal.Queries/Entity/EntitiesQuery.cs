using CleanModels.Commands;
using CleanModels.Queries.Base;
using Dal.Entities;
using Dal.Extensions;
using Dal.Queries.Base;
using Microsoft.EntityFrameworkCore;

namespace Dal.Queries;

/// <inheritdoc />
public class EntitiesQuery<TEntity, TModel> : DbQueryBase<CommonDbQueryModel<TModel>?, List<TEntity>>
    where TEntity : class, IEntity, new()
    where TModel : new()
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EntitiesQuery{TEntity, TModel}"/> class.
    /// </summary>
    /// <param name="contextFactory">Context factory.</param>
    public EntitiesQuery(IDbContextFactory<Context> contextFactory)
        : base(contextFactory)
    {
    }

    /// <inheritdoc/>
    protected override async Task<QueryResult<List<TEntity>>> ExecuteCoreAsync(CommonDbQueryModel<TModel>? model)
    {
        var entities = await GetEntities(model?.Models, model?.UniqueValuePropertyNames, model?.PropertyToIncludePath);

        return GetSuccessfulResult(entities);
    }

    private async Task<List<TEntity>> GetEntities(
        List<TModel?>? uniqueValuesForSearch,
        string[]? propertyNames,
        string? propertyToIncludePath)
    {
        if (propertyNames?.Length > 1)
        {
            throw new NotImplementedException();
        }

        // after that step maybe parallelism or, what better, one command (context) on one thread
        var dbSet = Context.Set<TEntity>().AsNoTracking();
        if (!string.IsNullOrWhiteSpace(propertyToIncludePath))
        {
            dbSet = dbSet.IncludeByPropertyPath(propertyToIncludePath);
        }

        if (propertyNames?.Length == 1 && uniqueValuesForSearch != null)
        {
            dbSet = dbSet.Filter(propertyNames[0], uniqueValuesForSearch);
        }

        var entities = await dbSet.ToListAsync();

        return entities;
    }
}