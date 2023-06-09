using System.Globalization;
using CleanModels.Queries.Base;
using Client.Services.Base;
using CsvHelper;
using CsvHelper.Configuration;

namespace Client.Services.Parsers;

public class FileParser<TResult, TMapper> : IParser<TResult, TMapper>
    where TMapper : ClassMap
{
    private readonly CsvConfiguration configuration = new (CultureInfo.InvariantCulture) // https://github.com/JoshClose/CsvHelper/blob/master/src/CsvHelper/Configuration/IReaderConfiguration.cs
    {
        Delimiter = " |#| ",
        // BadDataFound = null,
    };

    public QueryResult<List<TResult>> Parse(string filePath) // https://joshclose.github.io/CsvHelper/getting-started/
    {
        string fullPath = Path.GetFullPath(filePath);
        using var streamReader = new StreamReader(fullPath);
        using var csvReader = new CsvReader(streamReader, configuration);
        csvReader.Context.RegisterClassMap<TMapper>();
        var records = csvReader.GetRecordsAsync<TResult>().ToBlockingEnumerable().ToList();
        // var records = new List<TResult>();
        // while (csvReader.Read())
        // {
        //     var record = csvReader.GetRecord<TResult>();
        //     if (record is null)
        //     {
        //         const string errorMessage = @"Record is empty.";
        //
        //         return new QueryResult<List<TResult>>(errorMessage);
        //     }
        //
        //     records.Add(record);
        // }

        return new QueryResult<List<TResult>>(data: records);
    }
}