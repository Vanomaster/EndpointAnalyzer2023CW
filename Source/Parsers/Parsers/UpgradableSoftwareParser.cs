using CleanModels.Queries.Base;
using CleanModels.Software;
using Parsers.Base;

namespace Parsers;

public class UpgradableSoftwareParser : IParser<string, List<UpgradableSoftware>>
{
    private const string AllSoftwareIsUpToDateText = @"Все пакеты имеют последние версии.";
    private const string PreStartLineStart = "Вывод списка…";
    //private const int LinesToSkip = 4;
    private static readonly string[] Separators = { "\r\n", "\r", "\n" };

    /// <inheritdoc/>
    public QueryResult<List<UpgradableSoftware>> Parse(string model)
    {
        if (string.IsNullOrWhiteSpace(model))
        {
            return new QueryResult<List<UpgradableSoftware>>("Text to parse is empty.");
        }

        if (model == AllSoftwareIsUpToDateText)
        {
            return new QueryResult<List<UpgradableSoftware>>(new List<UpgradableSoftware>());
        }

        string[] lines = model.Trim().Split(Separators, StringSplitOptions.TrimEntries);
        int linesToSkip = Array.FindIndex(lines, line => line.StartsWith(PreStartLineStart)) + 1;
        var mainLines = new ArraySegment<string>(lines, linesToSkip, lines.Length - linesToSkip);
        //var mainLines = new ArraySegment<string>(lines, LinesToSkip, lines.Length - LinesToSkip);
        var upgradableSoftware = mainLines
            .Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .Select(words => new UpgradableSoftware
            {
                Name = words[0].Split('/')[0],
                CurrentVersion = words[^1].Trim(']'),
                NewVersion = words[1],
            })
            .ToList();

        return new QueryResult<List<UpgradableSoftware>>(upgradableSoftware);
    }
}