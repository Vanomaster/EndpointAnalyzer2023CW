using CleanModels.Queries.Base;

namespace Parsers.Base;

/// <summary>
/// Parser.
/// </summary>
/// <typeparam name="TModel">Type of model.</typeparam>
/// <typeparam name="TResult">Type of result.</typeparam>
public interface IParser<TModel, TResult>
{
    /// <summary>
    /// Parse model to result.
    /// </summary>
    /// <param name="model">Model.</param>
    /// <returns>Result.</returns>
    QueryResult<TResult> Parse(TModel model);
}