using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models
{
    public class DownloadStartedEventArgs : EventArgs
    {
        public DownloadStartedEventArgs(FileEntry fileEntry)
        {
            if (fileEntry is null)
            {
                throw new ArgumentNullException(nameof(fileEntry));
            }

            this.FileName = fileEntry?.Name ?? throw new ArgumentNullException("FileEntry Name");
            this.Hash = fileEntry?.Hash ?? throw new ArgumentNullException("FileEntry Hash");
        }

        public string FileName { get; }

        public string Hash { get; }
    }
}
