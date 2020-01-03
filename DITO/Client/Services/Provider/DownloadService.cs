using Client.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Torrent;

namespace Client.Services.Provider
{
    public class DownloadService : IDownloadService
    {
        public event EventHandler DownloadStarted;

        public void AddDownload(IEnumerable<Task<FileReply>> download)
        {
        }
    }
}
