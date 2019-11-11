using Grpc.Core;
using Server;

namespace Client.Services.Provider
{
    public class ClientToClientService
    {
        public ClientToClientService()
        {
            var port = 5001;
            var channel = new Channel("localhost:5001", ChannelCredentials.Insecure);

            var client = new Greeter.GreeterClient(channel);
            var reply = client.SayHello(new HelloRequest {  Name = "alex"});

        }
    }
}
