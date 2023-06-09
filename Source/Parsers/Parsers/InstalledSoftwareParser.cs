using CleanModels.Queries.Base;
using CleanModels.Software;
using Parsers.Base;

namespace Parsers;

public class InstalledSoftwareParser : IParser<string, List<Software>>
{
    //private const int LinesToSkip = 4;
    private const string PreStartLineStart = "+++-";
    private const string InstalledSoftwareSymbol = "ii"; // maybe add Status prop in software model, but need converter.
    private static readonly string[] Separators = { "\r\n", "\r", "\n" };

    /// <inheritdoc/>
    public QueryResult<List<Software>> Parse(string model)
    {
        if (string.IsNullOrWhiteSpace(model))
        {
            return new QueryResult<List<Software>>("Text to parse is empty.");
        }

        string[] lines = model.Split(Separators, StringSplitOptions.TrimEntries);
        int linesToSkip = Array.FindIndex(lines, line => line.StartsWith(PreStartLineStart)) + 1;
        var mainLines = new ArraySegment<string>(lines, linesToSkip, lines.Length - linesToSkip);
        var upgradableSoftware = mainLines
            .Where(line => line.StartsWith(InstalledSoftwareSymbol))
            .Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .Select(words => new Software
            {
                Name = words[1],
                Version = words[2],
                Description = string.Join(' ', new ArraySegment<string>(words, 4, words.Length - 4)),
            })
            .ToList();

        return new QueryResult<List<Software>>(upgradableSoftware);
    }
}