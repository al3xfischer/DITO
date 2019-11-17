using Grpc.Core;
using Server.Models;
using Server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            if (context.Peer.Substring(0, 5) == "ipv4:")
            {                
                this.torrentFileManagerService.RemoveTorrentFiles(request.DeletionFiles, context.Peer.Substring(5), request.ClientPort);          
            }

            return Task.FromResult(new Empty());
        }
    }
}
