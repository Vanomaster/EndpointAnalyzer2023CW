using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Dal.Extensions;

public static class ModelExtensions
{
    public static Dictionary<object, TModel> GetModelByUniqueValue<TModel>(
        this List<TModel?> models,
        string[] uniqueValuePropertyNames)
    {
        var uniqueValueProperties = uniqueValuePropertyNames
            .Select(GetTypePropertyByName<TModel>)
            .ToArray();

        var modelByUniqueValue = new Dictionary<object, TModel>();
        foreach (var model in models)
        {
            if (model == null)
            {
                throw new ArgumentException("Model is null.");
            }

            string uniqueValue = uniqueValueProperties
                .Aggregate(string.Empty, (current, property) => current + model.GetPropertyValue(property));

            modelByUniqueValue.Add(uniqueValue, model);
        }

        return modelByUniqueValue;
    }

    public static TEntity GetEntity<TModel, TEntity>(this TModel model)
        where TModel : new()
        where TEntity : new()
    {
        var entity = new TEntity();
        var modelProperties = typeof(TModel).GetProperties();
        foreach (var property in modelProperties)
        {
            var entityProperty = GetTypePropertyByName<TEntity>(property.Name);
            if (entityProperty.GetSetMethod() == null)
            {
                continue;
            }

            object? value = property.GetValue(model);
            entityProperty.SetValue(entity, value);
        }

        return entity;
    }

    public static PropertyInfo GetTypePropertyByName<TModel>(this string propertyName)
    {
        var property = typeof(TModel).GetProperty(propertyName);
        if (property != null)
        {
            return property;
        }

        string[] propertyNames = propertyName.Split('.');
        foreach (string name in propertyNames)
        {
            if (property == null)
            {
                property = typeof(TModel).GetProperty(name);

                continue;
            }

            property = property.PropertyType.GetProperty(name);
        }

        if (property == null)
        {
            throw new ArgumentException($"Property {propertyName} can't be got using {typeof(TModel)}.");
        }

        return property;
    }

    public static object? GetPropertyValue<TModel>([DisallowNull] this TModel model, PropertyInfo property)
    {
        if (property.DeclaringType?.Name != model.GetType().Name)
        {
            string declaringTypePropertyName = property.DeclaringType?.Name!;
            var declaringTypeProperty = GetTypePropertyByName<TModel>(declaringTypePropertyName);
            object? declaringTypeValue = declaringTypeProperty.GetValue(model);
            object? propertyValue = property.GetValue(declaringTypeValue);

            return propertyValue;
        }

        object? value = property.GetValue(model);

        return value;
    }
}