using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Torrent;

namespace Server.Services.Interfaces
{
    public interface ITorrentFileManagerService
    {
        IEnumerable<TorrentFile> GetAllTorrentFiles();

        void AddTorrentFile(SentFile file, string ipAddress, int port);

        void RemoveTorrentFile(SentFile file, string ipAddress, int port);
    }
}
