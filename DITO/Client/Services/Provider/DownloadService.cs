using Client.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Torrent;
using Google.Protobuf;
using System.IO;
using Client.Models;
using System.Linq;
using Grpc.Core;
using System.Collections.Concurrent;

namespace Client.Services.Provider
{
    public class DownloadService : IDownloadService
    {
        public event EventHandler<DownloadCompletedEventArgs> DownloadCompleted;
        public event EventHandler<DownloadStartedEventArgs> DownloadStarted;

        private readonly ConcurrentDictionary<string, IEnumerable<FileReply>> files;

        public ConcurrentDictionary<string, IEnumerable<FileReply>> Files
        {
            get => this.files;
        }

        public async void AddDownload(IEnumerable<FileRequest> requests, FileEntry file, IEnumerable<Host> hosts)
        {
            if (requests is null)
            {
                throw new ArgumentNullException(nameof(requests));
            }

            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            this.DownloadStarted?.Invoke(this, new DownloadStartedEventArgs(file));

            //var fileReplies = await Task.WhenAll(downloads);

            var workers = Worker<FileRequest, FileReply>.GetWorkers(hosts, (fileRequest, host) =>
            {
                var channel = new Channel(host.Name, host.Port, ChannelCredentials.Insecure);
                var client = new TorrentFileService.TorrentFileServiceClient(channel);
                //var batchLength = (host.i < partsCount - 1 ? this.configurationService.MaxBatchSize : lastBatchSize );
                return client.GetFileAsync(fileRequest).ResponseAsync;
            });

            Queue<FileRequest> queue = new Queue<FileRequest>(requests);

            List<FileReply> replies = new List<FileReply>();

            foreach (var worker in workers)
            {
                if (queue.Count == 0) break;

                worker.ResultReady += (sender, args) =>
                {
                    replies.Add(worker.Result);
                    if (queue.Count > 0)
                    {
                        worker.Execute(queue.Dequeue());
                    }

                    if (queue.Count == 0 && workers.All(w => !w.Running))
                    {
                        if (this.files.Keys.Contains(file.Hash))
                        {
                            return;
                        }

                        this.files.TryAdd(file.Hash, replies);
                        this.DownloadCompleted?.Invoke(this, new DownloadCompletedEventArgs(file.Hash,file.Name));
                    }

                };

                worker.Execute(queue.Dequeue());
            }
        }


    }
}
