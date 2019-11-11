using Grpc.Core;
using Server;

namespace Client.Services.Provider
{
    public class ClientToClientService
    {
        private readonly Greeter.GreeterClient client;
        public ClientToClientService()
        {
            var port = 5001;
            var channel = new Channel("10.0.0.4:5001", ChannelCredentials.Insecure);
            client = new Greeter.GreeterClient(channel);
        }

        public string CallServer()
        {
             return client.SayHello(new HelloRequest {  Name = "alex"}).Message;
        }
    }
}
