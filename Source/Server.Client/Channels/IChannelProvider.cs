using Grpc.Net.Client;

namespace Server.Client;

public interface IChannelProvider
{
    private const byte ChannelsNumber = 3;

    public GrpcChannel AcquireChannel(string pcIp);

    public void ReleaseChannel(string pcIp, GrpcChannel channel);

    public void InitChannelsForIp(string pcIp, byte channelsNumber = ChannelsNumber);

    public void RemoveChannelsForIp(string pcIp);
}