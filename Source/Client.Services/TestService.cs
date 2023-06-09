using CleanModels.Commands.Base;
using Client.Clients;
using NLog;

namespace Client.Services;

public class TestConnectionService
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Initializes a new instance of the <see cref="TestConnectionService"/> class.
    /// </summary>
    /// <param name="testServiceClient">TestServiceClient.</param>
    public TestConnectionService(TestServiceClient testServiceClient)
    {
        TestServiceClient = testServiceClient;
    }

    private TestServiceClient TestServiceClient { get; }

    public async Task<CommandResult> TestConnectionAsync()
    {
        Logger.Info("TestConnectionService TestConnection started.");
        var commandResult = await TestServiceClient.TestAsync();
        if (!commandResult.IsSuccessful)
        {
            Logger.Error($"TestConnectionService TestConnection failed. {commandResult.ErrorMessage}");

            return new CommandResult(commandResult.ErrorMessage);
        }

        Logger.Info("TestConnectionService TestConnection successfully completed.");

        return new CommandResult();
    }
}