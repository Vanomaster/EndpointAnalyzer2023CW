using Grpc.Net.Client;

namespace OsService.Client.Channels;

public interface IChannelProvider
{
    public GrpcChannel AcquireChannel();

    public void ReleaseChannel(GrpcChannel channel);
}