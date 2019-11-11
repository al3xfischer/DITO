using System;
using System.Collections.Generic;
using System.Text;
using Grpc.Core;
using System.Threading.Tasks;
using Server;

namespace Client.Services.Provider
{
    class GreeterIml : Greeter.GreeterBase
    {
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply { Message = "Hello" + request.Name });
        }
    }

    public class ClientServerService
    {
        public ClientServerService()
        {
            var port = 5001;
            var server = new Grpc.Core.Server
            {
                Services = { Greeter.BindService(new GreeterIml()) },
                Ports = { new ServerPort("localhost", port, ServerCredentials.Insecure) }
            };

            server.Start();
        }
    }
}
