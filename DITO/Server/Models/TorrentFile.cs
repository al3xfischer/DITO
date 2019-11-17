using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Server.Models
{
    public class TorrentFile
    {
        public TorrentFile(string fileHash)
        {
            FileHash = fileHash ?? throw new ArgumentNullException(nameof(fileHash));
        }

        public TorrentFile(string fileName, string fileHash, long fileSize, string ipAddress, int port)
        {
            FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
            FileHash = fileHash ?? throw new ArgumentNullException(nameof(fileHash));
            FileSize = fileSize;

            this.ClientAddresses = new List<IPEndPoint>();
            var address = IPEndPoint.Parse(ipAddress);
            address.Port = port;
            this.ClientAddresses.Add(address);
        }

        public string FileName { get; }

        public string FileHash { get; }

        public long FileSize { get; }

        public List<IPEndPoint> ClientAddresses { get; }

        public override bool Equals(object obj)
        {
            return (obj as TorrentFile).FileHash == this.FileHash;
        }
    }
}
