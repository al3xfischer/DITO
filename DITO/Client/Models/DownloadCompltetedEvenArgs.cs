using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Client.Models
{
    public class DownloadCompltetedEvenArgs : EventArgs
    {
        public DownloadCompltetedEvenArgs(FileInfo fileInfo)
        {
            this.FileInfo = fileInfo ?? throw new ArgumentNullException(nameof(fileInfo));
        }

        public FileInfo FileInfo { get; }
    }
}
