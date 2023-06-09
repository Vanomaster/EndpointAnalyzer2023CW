using System.Collections.Concurrent;
using Grpc.Net.Client;
using NLog;

namespace Server.Client;

/// <inheritdoc />
public class ChannelProvider : IChannelProvider
{
    private const string OsServicePort = "12124";
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private static readonly HttpClientHandler HttpClientHandler = new ()
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
    };

    /// <summary>
    /// Initializes a new instance of the <see cref="ChannelProvider"/> class.
    /// </summary>
    public ChannelProvider()
    {
        ChannelsByAddresses = new ConcurrentDictionary<string, BlockingCollection<GrpcChannel>>();
    }

    private ConcurrentDictionary<string, BlockingCollection<GrpcChannel>> ChannelsByAddresses { get; }

    /// <inheritdoc/>
    public GrpcChannel AcquireChannel(string pcIp)
    {
        string address = GetAddress(pcIp);
        if (!ChannelsByAddresses.TryGetValue(address, out var channels))
        {
            throw new ArgumentException($"Channels for {nameof(address)} = {address} not exist.");
        }

        return channels.Take();
    }

    /// <inheritdoc/>
    public void ReleaseChannel(string pcIp, GrpcChannel channel)
    {
        string address = GetAddress(pcIp);
        if (!ChannelsByAddresses.ContainsKey(address))
        {
            throw new ArgumentException($"{nameof(address)} = {address} not exist in ChannelsByAddresses.");
        }

        ChannelsByAddresses[address].Add(channel);
    }

    /// <inheritdoc/>
    public void InitChannelsForIp(string pcIp, byte channelsNumber)
    {
        string address = GetAddress(pcIp);
        if (ChannelsByAddresses.ContainsKey(address))
        {
            return;
        }

        var channels = new BlockingCollection<GrpcChannel>();
        for (byte i = 1; i <= channelsNumber; i++)
        {
            var channel = GetChannelForAddress(address);
            channels.Add(channel);
        }

        ChannelsByAddresses.TryAdd(address, channels);
    }

    /// <inheritdoc/>
    public void RemoveChannelsForIp(string pcIp)
    {
        string address = GetAddress(pcIp);
        ChannelsByAddresses.TryRemove(address, out _);
    }

    private static string GetAddress(string pcIp)
    {
        return $"http://{pcIp}:{OsServicePort}";
    }

    private static GrpcChannel GetChannelForAddress(string address)
    {
        var channelOptions = new GrpcChannelOptions { HttpHandler = HttpClientHandler };
        var channel = GrpcChannel.ForAddress(address, channelOptions);

        return channel;
    }
}