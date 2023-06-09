using CleanModels.Commands.Base;
using Client.Clients.Base;
using Grpc.Core;
using TestService;
using static TestService.Test;

namespace Client.Clients;

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
        TestResponse testResponse;
        try
        {
            testResponse = await Client.TestAsync(testRequest, deadline: DateTime.UtcNow.AddSeconds(3));
        }
        catch
        {
            return new CommandResult("Test failed. Server deadline exceeded.");
        }

        bool responseEqualsRequest = testResponse.Ids.All(id => ids.Contains(id));
        if (!responseEqualsRequest)
        {
            return new CommandResult("Test failed. Response not equals request.");
        }

        return new CommandResult();
    }
}