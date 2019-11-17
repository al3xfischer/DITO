using Grpc.Core;
using Server.Services.Interfaces;
using System;
using System.Threading.Tasks;
using Torrent;
using Google.Protobuf.WellKnownTypes;

namespace Server.Services.Provider
{
    public class DeleteFilesServiceImpl : DeleteFilesService.DeleteFilesServiceBase
    {
        private readonly ITorrentFileManagerService torrentFileManagerService;

        public DeleteFilesServiceImpl(ITorrentFileManagerService torrentFileManagerService)
        {
            this.torrentFileManagerService = torrentFileManagerService ?? throw new ArgumentNullException(nameof(torrentFileManagerService));
        }

        public override Task<Empty> DeleteFiles(DeleteRequest request, ServerCallContext context)
        {
            var ip = context.GetHttpContext().Connection.RemoteIpAddress.ToString();
            this.torrentFileManagerService.RemoveTorrentFiles(request.DeletionFiles,ip, request.ClientPort);

            return Task.FromResult(new Empty());
        }
    }
}
