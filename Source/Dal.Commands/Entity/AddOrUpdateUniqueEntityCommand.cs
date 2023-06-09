using CleanModels.Commands;
using CleanModels.Commands.Base;
using Dal.Commands.Base;
using Dal.Entities;
using Dal.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Dal.Commands;

/// <inheritdoc />
public class AddOrUpdateUniqueEntityCommand<TEntity, TModel> : DbCommandBase<CommonDbQueryModel<TModel>>
    where TEntity : class, IEntity, new()
    where TModel : new()
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AddOrUpdateUniqueEntityCommand{TEntity, TModel}"/> class.
    /// </summary>
    /// <param name="contextFactory">Context factory.</param>
    public AddOrUpdateUniqueEntityCommand(IDbContextFactory<Context> contextFactory)
        : base(contextFactory)
    {
    }

    /// <inheritdoc/>
    protected override async Task<CommandResult> ExecuteCoreAsync(CommonDbQueryModel<TModel> model)
    {
        await AddOrUpdateUniqueEntitiesWithIncludeAsync(
            model.Models,
            model.UniqueValuePropertyNames,
            model.PropertyToIncludePath);

        await Context.SaveChangesAsync();

        return GetSuccessfulResult();
    }

    /// <summary>
    /// Add or update unique entities by models. Add entity if it not yet exist in db. Update entity if it exist in db.
    /// </summary>
    /// <param name="models">Models for add or update.</param>
    /// <param name="uniqueValuePropertyNames">Names of properties which are parts of unique value of entity.</param>
    /// <param name="propertyToIncludePath">Path of property that need to include.</param>
    /// <typeparam name="TEntity">Entity type.</typeparam>
    /// <typeparam name="TModel">Model type.</typeparam>
    private async Task AddOrUpdateUniqueEntitiesWithIncludeAsync(
        List<TModel?> models,
        string[] uniqueValuePropertyNames,
        string? propertyToIncludePath)
    {
        // after that step maybe parallelism or, what better, one command (context) on one thread
        var modelByUniqueValues = models.GetModelByUniqueValue(uniqueValuePropertyNames);
        var entitiesToUpdate = await GetEntitiesToUpdateAsync(
            modelByUniqueValues.Keys.ToList(),
            uniqueValuePropertyNames,
            propertyToIncludePath);

        var entityToUpdateByUniqueValues = entitiesToUpdate!.GetModelByUniqueValue(uniqueValuePropertyNames);
        await AddOrUpdateUniqueEntities(modelByUniqueValues, entityToUpdateByUniqueValues);
    }

    private async Task<List<TEntity>> GetEntitiesToUpdateAsync(
        List<object> modelUniqueValues,
        string[] uniqueValuePropertyNames,
        string? propertyToIncludePath)
    {
        var dbSet = Context.Set<TEntity>().AsNoTracking();
        if (!string.IsNullOrWhiteSpace(propertyToIncludePath))
        {
            dbSet = dbSet.IncludeByPropertyPath(propertyToIncludePath);
        }

        if (uniqueValuePropertyNames.Length == 1)
        {
            dbSet = dbSet.Filter(uniqueValuePropertyNames[0], modelUniqueValues);
        }

        var entities = await dbSet.ToListAsync();

        return entities;
    }

    private async Task AddOrUpdateUniqueEntities<TEntity, TModel>(
        Dictionary<object, TModel> modelByUniqueValues,
        Dictionary<object, TEntity> entityToUpdateByUniqueValues)
        where TEntity : class, IEntity, new()
        where TModel : new()
    {
        var models = modelByUniqueValues.Values.ToArray();
        var someModel = models[0];
        bool modelIsEntity = someModel!.GetType().GetInterfaces().Contains(typeof(IEntity));
        foreach (var model in models)
        {
            await AddOrUpdateUniqueEntity(model, modelIsEntity, modelByUniqueValues, entityToUpdateByUniqueValues);
        }
    }

    private async Task AddOrUpdateUniqueEntity<TModel, TEntity>(
        TModel model,
        bool modelIsEntity,
        IReadOnlyDictionary<object, TModel> modelByUniqueValues,
        IReadOnlyDictionary<object, TEntity> entityToUpdateByUniqueValues)
        where TEntity : class, IEntity, new()
        where TModel : new()
    {
        object modelUniqueValue = modelByUniqueValues.Keys
            .FirstOrDefault(key => modelByUniqueValues[key]?.Equals(model) ?? false) ?? string.Empty;

        if (!entityToUpdateByUniqueValues.TryGetValue(modelUniqueValue, out var entity))
        {
            // throw new NullReferenceException($"Key {modelUniqueValue} not found.");
        }

        bool modelIsToUpdate = entity != null;
        if (modelIsToUpdate)
        {
            Context.UpdateEntity(entity!, model!, modelIsEntity);

            return;
        }

        var entityByModel = model.GetEntity<TModel, TEntity>();
        await Context.AddEntity(entityByModel);
    }
}