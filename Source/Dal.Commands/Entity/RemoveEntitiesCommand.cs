using CleanModels.Commands.Base;
using Dal.Commands.Base;
using Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dal.Commands;

/// <inheritdoc />
public class RemoveEntitiesCommand<TEntity> : DbCommandBase<IEnumerable<TEntity>>
    where TEntity : class, IEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RemoveEntitiesCommand{TEntity}"/> class.
    /// </summary>
    /// <param name="contextFactory">Context factory.</param>
    public RemoveEntitiesCommand(IDbContextFactory<Context> contextFactory)
        : base(contextFactory)
    {
    }

    /// <inheritdoc/>
    protected override async Task<CommandResult> ExecuteCoreAsync(IEnumerable<TEntity> entities)
    {
        Context.RemoveRange(entities);
        await Context.SaveChangesAsync();

        return GetSuccessfulResult();
    }
}