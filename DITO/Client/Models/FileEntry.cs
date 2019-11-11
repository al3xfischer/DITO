using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models
{
    public class FileEntry
    {
        public string Name { get; set; }

        public int Length { get; set; }

        public string Hash { get; set; }
    }
}
