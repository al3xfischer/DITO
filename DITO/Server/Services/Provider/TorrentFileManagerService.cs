using Server.Models;
using Server.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Torrent;

namespace Server.Services.Provider
{
    public class TorrentFileManagerService : ITorrentFileManagerService
    {
        private ICollection<TorrentFile> torrentFiles;

        public TorrentFileManagerService()
        {
            this.torrentFiles = new List<TorrentFile>();
        }

        public void AddTorrentFile(SentFile file, string ipAddress, int port)
        {
            var existingFile = this.torrentFiles.FirstOrDefault(f => f.FileHash == file.FileHash && f.FileName == file.FileName);

            if (existingFile == null)
            {
                this.torrentFiles.Add(new TorrentFile(file.FileName, file.FileHash, file.FileSize, ipAddress, port));
            }
            else
            {
                var address = IPEndPoint.Parse(ipAddress);
                address.Port = port;

                var existingClient = existingFile.Clients.FirstOrDefault(c => c.ToString() == address.ToString());

                if (existingClient == null)
                {
                    existingFile.Clients.Add(address);
                }
            }
        }

        public IEnumerable<TorrentFile> GetAllTorrentFiles()
        {
            return this.torrentFiles;
        }

        public void RemoveTorrentFile(SentFile file, string ipAddress, int port)
        {
            var existingFile = this.torrentFiles.FirstOrDefault(f => f.FileHash == file.FileHash && f.FileName == file.FileName);

            if (existingFile != null)
            {
                var address = IPEndPoint.Parse(ipAddress);
                address.Port = port;

                if (existingFile.Clients.Count() == 1 && existingFile.Clients.First().ToString() == address.ToString())
                {
                    this.torrentFiles.Remove(existingFile);
                }
                else
                {
                    var existingClient = existingFile.Clients.FirstOrDefault(c => c.ToString() == address.ToString());

                    existingFile.Clients.Remove(existingClient);
                }
            }            
        }
    }
}
