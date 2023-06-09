using System.Collections.Concurrent;
using Grpc.Net.Client;

namespace OsService.Client.Channels;

public class ChannelProvider : IChannelProvider
{
    private const byte ChannelsNumber = 5;
    private readonly string serverAddress; // 10.1.0.1 or localhost
    private static readonly HttpClientHandler HttpClientHandler = new ()
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="ChannelProvider"/> class.
    /// </summary>
    /// <param name="serverAddress">Server address.</param>
    public ChannelProvider(string? serverAddress)
    {
        this.serverAddress = serverAddress ?? throw new ArgumentNullException(nameof(serverAddress));
        Channels = new BlockingCollection<GrpcChannel>();
        CreateChannels();
    }

    private BlockingCollection<GrpcChannel> Channels { get; }

    public GrpcChannel AcquireChannel()
    {
        return Channels.Take();
    }

    public void ReleaseChannel(GrpcChannel channel)
    {
        Channels.Add(channel);
    }

    private void CreateChannels()
    {
        for (byte i = 1; i <= ChannelsNumber; i++)
        {
            var channel = GetChannel();
            Channels.Add(channel);
        }
    }

    private GrpcChannel GetChannel()
    {
        var channelOptions = new GrpcChannelOptions { HttpHandler = HttpClientHandler };
        var channel = GrpcChannel.ForAddress(serverAddress, channelOptions);

        return channel;
    }
}