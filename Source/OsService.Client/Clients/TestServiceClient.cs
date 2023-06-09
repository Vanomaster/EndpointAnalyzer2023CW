using CleanModels.Commands.Base;
using OsService.Client.Clients.Base;
using TestService;
using static TestService.Test;

namespace OsService.Client.Clients;

/// <inheritdoc />
public class TestServiceClient : ServiceClientBase<TestClient>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestServiceClient"/> class.
    /// </summary>
    /// <param name="client">Client.</param>
    public TestServiceClient(TestClient client)
        : base(client)
    {
    }

    public async Task<CommandResult> TestAsync()
    {
        var ids = new[] { "1" };
        var testRequest = new TestRequest();
        testRequest.Ids.AddRange(ids);
        var testResponse = await Client.TestAsync(testRequest);
        bool responseEqualsRequest = testResponse.Ids.All(id => ids.Contains(id));
        if (!responseEqualsRequest)
        {
            return new CommandResult("Test failed. Response not equals request.");
        }

        return new CommandResult();
    }
}