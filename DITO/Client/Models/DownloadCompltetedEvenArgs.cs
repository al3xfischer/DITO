using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Client.Models
{
    public class DownloadCompltetedEvenArgs : EventArgs
    {
        public DownloadCompltetedEvenArgs(FileInfo info, bool success, string hash)
        {
            this.FileInfo = info ?? throw new ArgumentNullException(nameof(info));
            this.Success = success;
            this.Hash = hash ?? throw new ArgumentNullException(nameof(hash));
        }

        public FileInfo FileInfo { get; }

        public bool Success { get; }

        public string Hash { get; }
    }
}
