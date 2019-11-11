using System;
using Grpc.Core;
using System.Threading.Tasks;
using Client.Services.Interfaces;
using Torrent;

namespace Client.Services.Provider
{

    public class ClientServerService
    {
        private readonly IConfigurationService configurationService;

        private Grpc.Core.Server server;

        public ClientServerService(IConfigurationService configurationService,TorrentFileServiceImpl torrentFileService)
        {
            if (torrentFileService is null)
            {
                throw new ArgumentNullException(nameof(torrentFileService));
            }

            this.configurationService = configurationService ?? throw new ArgumentNullException(nameof(configurationService));

            server = new Grpc.Core.Server
            {
                Services = { TorrentFileService.BindService(torrentFileService)  },
                Ports = { new ServerPort("0.0.0.0", this.configurationService.LocalServerPort, ServerCredentials.Insecure) },
            };
        }

        public void Start() => this.server.Start();

        public Task Stop() => this.server.ShutdownAsync();
    }
}
