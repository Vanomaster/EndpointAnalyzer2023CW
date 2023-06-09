using Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dal.Extensions;

public static class ContextExtensions
{
    public static async Task AddEntity<TEntity>(this DbContext context, TEntity entity)
    {
        await context.AddAsync(entity!);
    }

    public static void UpdateEntity<TEntity>(this DbContext context, TEntity entityToUpdate, dynamic model, bool modelIsEntity)
        where TEntity : class, IEntity
    {
        if (modelIsEntity && model.Id == (Guid)default)
        {
            model.Id = entityToUpdate.Id;
        }

        context.Entry(entityToUpdate).CurrentValues.SetValues(model);
    }
}