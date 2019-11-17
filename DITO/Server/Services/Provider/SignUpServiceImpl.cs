using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Server.Models;
using Server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Torrent;

namespace Server.Services.Provider
{
    public class SignUpServiceImpl : SignUpService.SignUpServiceBase
    {
        private readonly ITorrentFileManagerService torrentFileManagerService;

        public SignUpServiceImpl(ITorrentFileManagerService torrentFileManagerService)
        {
            this.torrentFileManagerService = torrentFileManagerService ?? throw new ArgumentNullException(nameof(torrentFileManagerService));
        }

        public override Task<SignUpReply> SignUp(SignUpMessage request, ServerCallContext context)
        {
            SignUpReply reply = new SignUpReply();
            
            if (context.Peer.Substring(0, 5) == "ipv4:")
            {
                foreach (var torrentFile in this.torrentFileManagerService.GetAllTorrentFiles())
                {
                    var file = new TFile {
                        FileName = torrentFile.FileName,
                        FileHash = torrentFile.FileHash,
                        FileSize = torrentFile.FileSize,
                    };

                    file.ClientIPs.AddRange(torrentFile.ClientAddresses.Select(f => f.Address.ToString()));
                    file.ClientPorts.AddRange(torrentFile.ClientAddresses.Select(f => f.Port));

                    reply.TorrentFiles.Add(file);
                }

                this.torrentFileManagerService.AddTorrentFiles(request.Files,context.Peer.Substring(5), request.ClientPort);
            }
            
            return Task.FromResult(reply);
        }

        public override Task<Empty> SignUpOneFile(SignUpMessage request, ServerCallContext context)
        {
            if (context.Peer.Substring(0, 5) == "ipv4:")
            {
                this.torrentFileManagerService.AddTorrentFiles(request.Files, context.Peer.Substring(5), request.ClientPort);
            }

            return Task.FromResult(new Empty());
        }
    }
}
