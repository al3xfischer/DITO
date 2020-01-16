using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models
{
    public class Download
    {
        public string  FileName { get; set; }

        public bool Completed { get; set; }

        public DateTime CompletedTimeStamp { get; set; }

        public string Hash { get; set; }

        public bool Success { get; set; }
    }
}
