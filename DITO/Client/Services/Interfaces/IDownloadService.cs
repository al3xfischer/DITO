using Client.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Torrent;

namespace Client.Services.Interfaces
{
    public interface IDownloadService
    {
        public event EventHandler<DownloadStartedEventArgs> DownloadStarted;
        public event EventHandler<DownloadCompletedEventArgs> DownloadCompleted;

        public ConcurrentDictionary<string,IEnumerable<FileReply>> Files {get;}

        public void AddDownload(IEnumerable<FileRequest> requests, FileEntry file, IEnumerable<Host> hosts);
    }
}
