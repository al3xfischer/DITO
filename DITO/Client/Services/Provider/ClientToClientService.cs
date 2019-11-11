using Client.Models;
using Client.Services.Interfaces;
using Grpc.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Torrent;

namespace Client.Services.Provider
{
    public class ClientToClientService
    {
        private readonly IConfigurationService configurationService;

        public ClientToClientService(IConfigurationService configurationService)
        {
            this.configurationService = configurationService;
        }

        public IEnumerable<Task<FileReply>> QueryFile(IEnumerable<Host> hosts, FileEntry file)
        {
            if (hosts is null)
            {
                throw new System.ArgumentNullException(nameof(hosts));
            }

            if (file is null)
            {
                throw new System.ArgumentNullException(nameof(file));
            }

            var partsCount = file.Length / this.configurationService.MaxBatchSize;
            var lastBatchSize = file.Length % this.configurationService.MaxBatchSize;
            if (lastBatchSize != 0) partsCount++;
            var hots = hosts.Repeat((i) => i <= partsCount);

            foreach (var host in hosts.Select((config, i) => (config, i)))
            {
                var channel = new Channel(host.config.Name, host.config.Port, ChannelCredentials.Insecure);
                var client = new TorrentFileService.TorrentFileServiceClient(channel);
                var batchLength = (host.i < partsCount - 1 ? this.configurationService.MaxBatchSize : lastBatchSize );
                yield return client.GetFileAsync(new FileRequest
                {
                    Index = host.i,
                    MaxBatchSize = (int)this.configurationService.MaxBatchSize,
                    Name = file.Name,
                    BatchLength = batchLength
                }).ResponseAsync;
            }
        }
    }
}
