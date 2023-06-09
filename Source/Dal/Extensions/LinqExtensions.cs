using System.Linq.Expressions;
using Dal.Entities;
using Microsoft.EntityFrameworkCore;

namespace Dal.Extensions;

public static class LinqExtensions
{
    public static IQueryable<TSource> Filter<TSource, TModel>(
        this IQueryable<TSource> source,
        string propertyName,
        List<TModel> uniqueValues)
    {
        var entity = Expression.Parameter(typeof(TSource), "entity");
        var entityProperty = Expression.Property(entity, propertyName);
        var type = uniqueValues.GetType();
        var containsMethod = type.GetMethod("Contains");
        Expression<Func<List<TModel>>> uniqueValuesParameterLambda = () => uniqueValues;
        var uniqueValuesParameterExpression = uniqueValuesParameterLambda.Body;
        var call = Expression.Call(uniqueValuesParameterExpression, containsMethod!, entityProperty);
        var filter = Expression.Lambda<Func<TSource, bool>>(call, entity);

        return source.Where(filter);
    }

    public static IQueryable<TEntity> IncludeByPropertyPath<TEntity>(
        this IQueryable<TEntity> source,
        string propertyPath)
        where TEntity : class, IEntity
    {
        string propertyName = propertyPath.Split('.')[0];
        var property = propertyName.GetTypePropertyByName<TEntity>();
        var propertyAccessor = property.GetAccessors()[0];
        if (!propertyAccessor.IsVirtual)
        {
            throw new ArgumentException($"Property {propertyName} on type {typeof(TEntity)} can not be included " +
                                        "because it is not virtual property.");
        }

        if (propertyAccessor.IsVirtual)
        {
            source = source.Include(propertyName);
        }

        return source;
    }

    public static IQueryable<TSource> UsingGroupDistinctBy<TSource, TKey>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, TKey>> keySelector)
    {
        return source.GroupBy(keySelector).Select(x => x.FirstOrDefault());
    }
}