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
        ICollection<TorrentFile> GetAllTorrentFiles();

        void AddTorrentFiles(IEnumerable<SentFile> torrentFiles, string ipAddress, int port);

        void RemoveTorrentFiles(IEnumerable<DeletionFile> torrentFiles, string ipAddress, int port);
    }
}
