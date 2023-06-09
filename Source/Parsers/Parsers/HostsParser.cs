using CleanModels.Network;
using CleanModels.Queries.Base;
using Parsers.Base;

namespace Parsers;

public class HostsParser : IParser<string, List<Host>>
{
    //private const int LinesToSkip = 5;
    private const string StartLineStart = "Discovered open port";//"# Ports scanned";
    private const string RateLineStart = "rate: ";//"# Ports scanned";
    private static readonly string[] Separators = { "\r\n", "\r", "\n" };

    /// <inheritdoc/>
    public QueryResult<List<Host>> Parse(string model)
    {
        if (string.IsNullOrWhiteSpace(model))
        {
            return new QueryResult<List<Host>>("Text to parse is empty.");
        }

        if (!model.Contains(StartLineStart))
        {
            return new QueryResult<List<Host>>(new List<Host>());
        }

        string[] lines = model.Split(Separators, StringSplitOptions.TrimEntries);
        lines = lines.Where(l => l.Contains(StartLineStart)).ToArray();
        //int linesToSkip = Array.FindIndex(lines, line => line.StartsWith(StartLineStart));
        //int linesToGo = Array.FindIndex(lines, line => line.StartsWith(RateLineStart));
        //var mainLines = new ArraySegment<string>(lines, linesToSkip, lines.Length - linesToSkip);
        var hosts = lines
            .Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            .Select(words => new Host
            {
                Ip = words[5],
            })
            .ToList();

        return new QueryResult<List<Host>>(hosts);
    }
}