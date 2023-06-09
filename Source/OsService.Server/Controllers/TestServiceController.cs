using Grpc.Core;
using NLog;
using TestService;

namespace OsService.Server.Controllers;

/// <inheritdoc />
public class TestServiceController : Test.TestBase
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    /// <inheritdoc/>
    public override async Task<TestResponse> Test(TestRequest model, ServerCallContext context)
    {
        var response = await GetTestResponse(model);

        return response;
    }

    private static Task<TestResponse> GetTestResponse(TestRequest model)
    {
        var response = new TestResponse();
        response.Ids.AddRange(model.Ids);
        Logger.Debug("Test on os service successfully completed.");

        return Task.FromResult(response);
    }
}