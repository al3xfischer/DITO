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
    public class FileRequestServiceImpl : FileRequestService.FileRequestServiceBase
    {
        private readonly ITorrentFileManagerService torrentFileManagerService;

        public FileRequestServiceImpl(ITorrentFileManagerService torrentFileManagerService)
        {
            this.torrentFileManagerService = torrentFileManagerService ?? throw new ArgumentNullException(nameof(torrentFileManagerService));
        }

        public override Task<FileRequestReply> RequestFiles(Empty request, ServerCallContext context)
        {
            var files = this.torrentFileManagerService.GetAllTorrentFiles();

            FileRequestReply reply = new FileRequestReply();

            foreach (var file in files)
            {
                var torrentFile = new RequestedTorrentFile();
                torrentFile.FileName = file.FileName;
                torrentFile.FileHash = file.FileHash;
                torrentFile.FileSize = file.FileSize;

                torrentFile.ClientIPs.Add(file.Clients.Select(c => c.Address.ToString()));

                reply.Files.Add(torrentFile);
            }

            return Task.FromResult(reply);
        }
    }
}
