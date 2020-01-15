using Client.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Torrent;

namespace Client.Services.Interfaces
{
    public interface IDownloadService
    {
        public event EventHandler<DownloadStartedEventArgs> DownloadStarted;
        public event EventHandler<DownloadCompltetedEvenArgs> DownloadCompleted;

        public void AddDownload(IEnumerable<Task<FileReply>> downloads, FileEntry file);
    }
}
