using CleanModels.Queries.Base;
using Dal.Entities;
using Dal.Queries.Base;
using Microsoft.EntityFrameworkCore;

namespace Dal.Queries;

/// <inheritdoc />
public class AllQuery<TEntity> : DbQueryBase<List<TEntity>>
    where TEntity : class, IEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AllQuery{TEntity}"/> class.
    /// </summary>
    /// <param name="contextFactory">Context factory.</param>
    protected AllQuery(IDbContextFactory<Context> contextFactory)
        : base(contextFactory)
    {
    }

    /// <inheritdoc/>
    protected override async Task<QueryResult<List<TEntity>>> ExecuteCoreAsync()
    {
        var entities = await Context.Set<TEntity>().AsNoTracking().ToListAsync();

        return GetSuccessfulResult(entities);
    }
}