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

namespace Client.Services.Provider
{
    public class DownloadService : IDownloadService
    {
        public event EventHandler<DownloadCompltetedEvenArgs> DownloadCompleted;
        public event EventHandler<DownloadStartedEventArgs> DownloadStarted;

        private readonly IFileService fileService;
        private string filesFolder;

        public DownloadService(IFileService fileService)
        {
            this.fileService = fileService;
            this.filesFolder = Path.Combine(Directory.GetCurrentDirectory(), "shared");
        }

        public async void AddDownload(IEnumerable<Task<FileReply>> downloads, FileEntry file)
        {
            if (downloads is null)
            {
                throw new ArgumentNullException(nameof(downloads));
            }

            if (file is null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            this.DownloadStarted?.Invoke(this, new DownloadStartedEventArgs(file));
            
            var fileReplies = await Task.WhenAll(downloads);
            FileInfo fileInfo = this.fileService.SaveFile(this.MergePayloads(fileReplies), this.filesFolder, file.Name);
            var downloadedHash = this.fileService.GetHash(fileInfo);

            this.DownloadCompleted?.Invoke(this, new DownloadCompltetedEvenArgs(fileInfo,downloadedHash == file.Hash,file.Hash));
        }

        private byte[] MergePayloads(IEnumerable<FileReply> fileReplies)
        {
            return fileReplies.SelectMany(fr => fr.Payload).ToArray();
        }
    }
}
