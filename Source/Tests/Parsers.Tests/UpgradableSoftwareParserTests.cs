using Newtonsoft.Json;
using Xunit;

namespace Parsers.Tests;

public class UpgradableSoftwareParserTests
{
    [Theory]
    [InlineData("TestData/UpgradableSoftwareParser/Text.txt", "TestData/UpgradableSoftwareParser/Model.txt")]
    public async Task Parse_Text_ReturnsModels(string textPath, string modelPath)
    {
        string modelFilePath = Path.GetFullPath(modelPath);
        string expected = await File.ReadAllTextAsync(modelFilePath);
        string textFilePath = Path.GetFullPath(textPath);
        string text = await File.ReadAllTextAsync(textFilePath);
        var parser = new UpgradableSoftwareParser();

        var queryResult = parser.Parse(text);
        if (!queryResult.IsSuccessful)
        {
            Assert.Fail($"Test failed. {queryResult.ErrorMessage}");
        }

        var upgradableSoftware = queryResult.Data;
        string actual = JsonConvert.SerializeObject(upgradableSoftware.FirstOrDefault());

        Assert.Equal(expected, actual);
    }
}