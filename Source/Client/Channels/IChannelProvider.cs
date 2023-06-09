using Grpc.Net.Client;

namespace Client;

public interface IChannelProvider
{
    public GrpcChannel AcquireChannel();

    public void ReleaseChannel(GrpcChannel channel);
}