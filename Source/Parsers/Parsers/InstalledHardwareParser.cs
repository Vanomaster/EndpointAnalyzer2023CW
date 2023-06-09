using CleanModels.Hardware;
using CleanModels.Queries.Base;
using Parsers.Base;

namespace Parsers;

public class InstalledHardwareParser : IParser<string, List<UnknownHardware>>
{
    private const string VendorIdParamName = "idVendor";
    private const string ProductIdParamName = "idProduct";
    private static readonly string[] Separators = { "\r\n", "\r", "\n" };

    /// <inheritdoc/>
    public QueryResult<List<UnknownHardware>> Parse(string model)
    {
        if (string.IsNullOrWhiteSpace(model))
        {
            return new QueryResult<List<UnknownHardware>>("Text to parse is empty.");
        }

        string[] lines = model.Trim().Split(Separators, StringSplitOptions.TrimEntries);
        var upgradableSoftware = lines
            .Select(line => new UnknownHardware
            {
                HardwareId = string.Concat(
                    line.AsSpan(
                        line.IndexOf(VendorIdParamName, StringComparison.Ordinal) + VendorIdParamName.Length + 1,
                        4),
                    ":",
                    line.AsSpan(
                        line.IndexOf(ProductIdParamName, StringComparison.Ordinal) + ProductIdParamName.Length + 1,
                        4)),
            })
            .ToList();

        return new QueryResult<List<UnknownHardware>>(upgradableSoftware);
    }
}