using CleanModels.Commands.Base;
using Dal.Commands.Base;
using Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dal.Commands;

/// <inheritdoc />
public class UpdateOnlyNameUniqueEntityCommand<TEntity> : DbCommandBase<TEntity>
    where TEntity : class, INamedEntity
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UpdateOnlyNameUniqueEntityCommand{TEntity}"/> class.
    /// </summary>
    /// <param name="contextFactory">Context factory.</param>
    public UpdateOnlyNameUniqueEntityCommand(IDbContextFactory<Context> contextFactory)
        : base(contextFactory)
    {
    }

    /// <inheritdoc/>
    protected override async Task<CommandResult> ExecuteCoreAsync(TEntity entityToAdd)
    {
        var entitiesToFetch = Context.Set<TEntity>().AsNoTracking();
        bool entityIsExisted = await entitiesToFetch
            .AnyAsync(entity => entity.Id != entityToAdd.Id && entity.Name == entityToAdd.Name);

        if (entityIsExisted)
        {
            return GetFailedResult(@"Элемент с таким названием уже существует.");
        }

        Context.Update(entityToAdd);
        await Context.SaveChangesAsync();

        return GetSuccessfulResult();
    }
}