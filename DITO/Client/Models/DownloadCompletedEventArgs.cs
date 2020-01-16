using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Client.Models
{
    public class DownloadCompletedEventArgs : EventArgs
    {
        public DownloadCompletedEventArgs(string hash,string fileName)
        {
            this.Hash = hash ?? throw new ArgumentNullException(nameof(hash));
            FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
        }

        public string Hash { get; }
        public string FileName { get; }
    }
}
