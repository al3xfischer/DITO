using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Torrent;

namespace Client.Services.Interfaces
{
    public interface IDownloadService
    {
        public event EventHandler DownloadStarted;

        public void AddDownload(IEnumerable<Task<FileReply>> download);
    }
}
