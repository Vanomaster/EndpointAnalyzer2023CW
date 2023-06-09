using Newtonsoft.Json;
using Xunit;

namespace Parsers.Tests;

public class InstalledHardwareParserTests
{
    [Theory]
    [InlineData("TestData/InstalledHardwareParser/Text.txt", "TestData/InstalledHardwareParser/Model.txt")]
    public async Task Parse_Text_ReturnsModels(string textPath, string modelPath)
    {
        string modelFilePath = Path.GetFullPath(modelPath);
        string expected = await File.ReadAllTextAsync(modelFilePath);
        string textFilePath = Path.GetFullPath(textPath);
        string text = await File.ReadAllTextAsync(textFilePath);
        var parser = new InstalledHardwareParser();

        var queryResult = parser.Parse(text);
        if (!queryResult.IsSuccessful)
        {
            Assert.Fail($"Test failed. {queryResult.ErrorMessage}");
        }

        var hardware = queryResult.Data;
        string actual = JsonConvert.SerializeObject(hardware);

        Assert.Equal(expected, actual);
    }
}