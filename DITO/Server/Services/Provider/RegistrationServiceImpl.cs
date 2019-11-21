using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Torrent;

namespace Server.Services.Provider
{
    public class RegistrationServiceImpl : RegisterFilesService.RegisterFilesServiceBase
    {
        private readonly ITorrentFileManagerService torrentFileManagerService;

        public RegistrationServiceImpl(ITorrentFileManagerService torrentFileManagerService)
        {
            this.torrentFileManagerService = torrentFileManagerService ?? throw new ArgumentNullException(nameof(torrentFileManagerService));
        }

        public override Task<Empty> RegisterFile(RegistrationMessage request, ServerCallContext context)
        {
            var ip = context.GetHttpContext().Connection.RemoteIpAddress.ToString();
            this.torrentFileManagerService.AddTorrentFile(request.File, ip, request.ClientPort);

            return Task.FromResult(new Empty());
        }

        public override Task<Empty> RegisterMultipleFiles(RegisterMultipleMessage request, ServerCallContext context)
        {
            var ip = context.GetHttpContext().Connection.RemoteIpAddress.ToString();

            foreach (var file in request.Files)
            {
                this.torrentFileManagerService.AddTorrentFile(file, ip, request.ClientPort);
            }

            return Task.FromResult(new Empty());
        }

        public override Task<Empty> DeregisterFile(RegistrationMessage request, ServerCallContext context)
        {
            var ip = context.GetHttpContext().Connection.RemoteIpAddress.ToString();
            this.torrentFileManagerService.RemoveTorrentFile(request.File, ip, request.ClientPort);

            return Task.FromResult(new Empty());
        }

        public override Task<Empty> DeregisterMultipleFiles(RegisterMultipleMessage request, ServerCallContext context)
        {
            var ip = context.GetHttpContext().Connection.RemoteIpAddress.ToString();

            foreach (var file in request.Files)
            {
                this.torrentFileManagerService.RemoveTorrentFile(file, ip, request.ClientPort);
            }

            return Task.FromResult(new Empty());
        }
    }
}
