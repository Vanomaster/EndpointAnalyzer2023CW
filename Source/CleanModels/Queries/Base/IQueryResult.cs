﻿namespace CleanModels.Queries.Base;

/// <summary>
/// Query result.
/// </summary>
/// <typeparam name="TResult">Type of data.</typeparam>
public interface IQueryResult<out TResult>
{
    /// <summary>
    /// Data.
    /// </summary>
    public TResult Data { get; }

    /// <summary>
    /// Error message.
    /// </summary>
    public string? ErrorMessage { get; }

    /// <summary>
    /// Whether executed successfully.
    /// </summary>
    public bool IsSuccessful { get; }
}