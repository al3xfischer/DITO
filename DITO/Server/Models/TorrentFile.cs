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

        public TorrentFile(TorrentFile file, IEnumerable<IPEndPoint> clients)
        {
            this.FileName = file.FileName;
            this.FileHash = file.FileHash;
            this.FileSize = file.FileSize;
            this.Clients = new List<IPEndPoint>(clients);
        }

        public TorrentFile(string fileName, string fileHash, long fileSize, string ipAddress, int port)
        {
            this.FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
            this.FileHash = fileHash ?? throw new ArgumentNullException(nameof(fileHash));
            this.FileSize = fileSize;

            this.Clients = new List<IPEndPoint>();

            var address = IPEndPoint.Parse(ipAddress);
            address.Port = port;
            this.Clients.Add(address);
        }

        public string FileName { get; }

        public string FileHash { get; }

        public long FileSize { get; }

        public List<IPEndPoint> Clients { get; }

        public override bool Equals(object obj)
        {
            return (obj as TorrentFile).FileHash == this.FileHash;
        }
    }
}
