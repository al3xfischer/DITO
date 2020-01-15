using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models
{
    public class DownloadStartedEventArgs : EventArgs
    {
        public DownloadStartedEventArgs(string fileName)
        {
            this.FileName = fileName ?? throw new ArgumentNullException(nameof(fileName));
        }

        public string FileName { get; }
    }
}
