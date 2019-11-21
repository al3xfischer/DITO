using Client.Services.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Torrent;

namespace Client.Services.Provider
{
    public class FileRequestServiceImpl : FileRequestService.FileRequestServiceClient
    {
        public FileRequestServiceImpl(IConfigurationService configurationService) : base(GrpcChannel.ForAddress("https" +
            "://" + configurationService.ServerName + ":" + configurationService.ServerPort, FileRequestServiceImpl.CreateOptions()))
        {
        }

        private static GrpcChannelOptions CreateOptions()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            var httpClient = new HttpClient(handler);

            return new GrpcChannelOptions { HttpClient = httpClient, };
        }

        public async Task<List<RequestedTorrentFile>> RequestFiles()
        {
            var response = await this.RequestFilesAsync(new Empty()).ResponseAsync;

            return response.Files.ToList();
        }
    }
}
