using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models
{
    public class FileEntry
    {
        public string Name { get; set; }

        public long Length { get; set; }

        public string Hash { get; set; }
    }
}
