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

        public void AddTorrentFiles(IEnumerable<SentFile> torrentFiles, string ipAddress, int port)
        {
            foreach (var file in torrentFiles)
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

                    if (!existingFile.ClientAddresses.Any(a => a.ToString() == address.ToString()))
                    {
                        existingFile.ClientAddresses.Add(address);
                    }
                }
            }
        }

        public ICollection<TorrentFile> GetAllTorrentFiles()
        {
            return this.torrentFiles;
        }

        public void RemoveTorrentFiles(IEnumerable<DeletionFile> torrentFiles, string ipAddress, int port)
        {
            foreach (var file in torrentFiles)
            {
                var existingFile = this.torrentFiles.FirstOrDefault(f => f.FileHash == file.FileHash && f.FileName == file.FileName);

                if (existingFile != null)
                {
                    var address = IPEndPoint.Parse(ipAddress);
                    address.Port = port;

                    if (existingFile.ClientAddresses.Count() == 1 && existingFile.ClientAddresses.First().ToString() == address.ToString())
                    {
                        this.torrentFiles.Remove(existingFile);
                    }
                    else
                    {
                        var existingAddress = existingFile.ClientAddresses.FirstOrDefault(a => a.ToString() == address.ToString());

                        existingFile.ClientAddresses.Remove(existingAddress);
                    }
                }
            }
        }
    }
}
