using Client.Services.Interfaces;
using Google.Protobuf;
using Grpc.Core;
using System.Threading.Tasks;
using Torrent;

namespace Client.Services.Provider
{
    public class TorrentFileServiceImpl : TorrentFileService.TorrentFileServiceBase
    {
        private readonly IFileService fileService;

        public TorrentFileServiceImpl(IFileService fileService)
        {
            this.fileService = fileService ?? throw new System.ArgumentNullException(nameof(fileService));
        }

        public override Task<FileReply> GetFile(FileRequest request, ServerCallContext context)
        {
            var fileEntry = fileService.GetFileEntry(request.Name);
            var payload = !(fileEntry is null) ? fileService.ReadFile(fileEntry, request.Index * request.MaxBatchSize, request.BatchLength) : new byte[0];

            return Task.FromResult(new FileReply { Index = request.Index, Name = request.Name, Payload = ByteString.CopyFrom(payload) });
        }
    }
}
