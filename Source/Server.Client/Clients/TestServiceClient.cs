// using CleanModels.Commands.Base;
// using Server.Client.Clients.Base;
// using TestService;
// using static TestService.Test;
//
// namespace Server.Client.Clients;
//
// /// <inheritdoc />
// public class TestServiceClient : ServiceClientBase<TestClient>
// {
//     /// <summary>
//     /// Initializes a new instance of the <see cref="TestServiceClient"/> class.
//     /// </summary>
//     /// <param name="serviceProvider">Service provider.</param>
//     /// <param name="channelProvider">Channel provider.</param>
//     public TestServiceClient(IServiceProvider serviceProvider, IChannelProvider channelProvider)
//         : base(serviceProvider, channelProvider)
//     {
//     }
//
//     protected override async Task<CommandResult> TestAsync()
//     {
//         var ids = new[] { "1" };
//         var testRequest = new TestRequest();
//         testRequest.Ids.AddRange(ids);
//         var testResponse = await Client.TestAsync(testRequest);
//         bool responseEqualsRequest = testResponse.Ids.All(id => ids.Contains(id));
//         if (!responseEqualsRequest)
//         {
//             return new CommandResult("Test failed. Response not equals request.");
//         }
//
//         return new CommandResult();
//     }
// }