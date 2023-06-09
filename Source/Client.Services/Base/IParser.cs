using CleanModels.Queries.Base;
using CsvHelper.Configuration;

namespace Client.Services.Base;

/// <summary>
/// Parser.
/// </summary>
/// <typeparam name="TModel">Type of model.</typeparam>
/// <typeparam name="TResult">Type of result.</typeparam>
public interface IParser<TResult, TMapper>
    where TMapper : ClassMap
{
    /// <summary>
    /// Parse model to result.
    /// </summary>
    /// <param name="model">Model.</param>
    /// <returns>Result.</returns>
    QueryResult<List<TResult>> Parse(string model);
}