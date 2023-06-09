using Newtonsoft.Json;
using Xunit;

namespace Parsers.Tests;

public class HostsParserTests
{
    [Theory]
    [InlineData("TestData/HostsParser/Text.txt", "TestData/HostsParser/Model.txt")]
    public async Task Parse_Text_ReturnsModels(string textPath, string modelPath)
    {
        string modelFilePath = Path.GetFullPath(modelPath);
        string expected = await File.ReadAllTextAsync(modelFilePath);
        string textFilePath = Path.GetFullPath(textPath);
        string text = await File.ReadAllTextAsync(textFilePath);
        var parser = new HostsParser();

        var queryResult = parser.Parse(text);
        if (!queryResult.IsSuccessful)
        {
            Assert.Fail($"Test failed. {queryResult.ErrorMessage}");
        }

        var hosts = queryResult.Data;
        string actual = JsonConvert.SerializeObject(hosts.FirstOrDefault());

        Assert.Equal(expected, actual);
    }
}