using CleanModels.Queries.Base;
using Dal.Entities;
using Dal.Queries.Base;
using Microsoft.EntityFrameworkCore;

namespace Dal.Queries;

/// <inheritdoc />
public class AllNamesQuery<TEntity> : DbQueryBase<List<string>>
    where TEntity : class, INamedEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AllNamesQuery{TEntity}"/> class.
    /// </summary>
    /// <param name="contextFactory">Context factory.</param>
    public AllNamesQuery(IDbContextFactory<Context> contextFactory)
        : base(contextFactory)
    {
    }

    /// <inheritdoc/>
    protected override async Task<QueryResult<List<string>>> ExecuteCoreAsync()
    {
        var entities = await Context.Set<TEntity>().AsNoTracking().Select(entity => entity.Name).ToListAsync();

        return GetSuccessfulResult(entities);
    }
}