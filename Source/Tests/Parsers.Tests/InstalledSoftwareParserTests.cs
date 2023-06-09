using Newtonsoft.Json;
using Xunit;

namespace Parsers.Tests;

public class InstalledSoftwareParserTests
{
    [Theory]
    [InlineData("TestData/InstalledSoftwareParser/Text.txt", "TestData/InstalledSoftwareParser/Model.txt")]
    public async Task Parse_Text_ReturnsModels(string textPath, string modelPath)
    {
        string modelFilePath = Path.GetFullPath(modelPath);
        string expected = await File.ReadAllTextAsync(modelFilePath);
        string textFilePath = Path.GetFullPath(textPath);
        string text = await File.ReadAllTextAsync(textFilePath);
        var parser = new InstalledSoftwareParser();

        var queryResult = parser.Parse(text);
        if (!queryResult.IsSuccessful)
        {
            Assert.Fail($"Test failed. {queryResult.ErrorMessage}");
        }

        var software = queryResult.Data;
        string actual = JsonConvert.SerializeObject(software);

        Assert.Equal(expected, actual);
    }
}