using Grpc.Core;
using NLog;
using TestService;

namespace Server.Controllers;

/// <inheritdoc />
public class TestServiceController : Test.TestBase
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    /// <summary>
    /// Initializes a new instance of the <see cref="TestServiceController"/> class.
    /// </summary>
    public TestServiceController()
    {
        Logger.Debug("Creating TestServiceController.");
    }

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
        Logger.Debug("Test on server successfully completed.");

        return Task.FromResult(response);
    }
}