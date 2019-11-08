using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;

namespace Client.Models
{
    public class DitoConfiguration : ICloneable
    {
        [JsonProperty(PropertyName = "serverName")]
        public string ServerName { get; set; }

        [JsonProperty(PropertyName = "serverPort")]
        public uint ServerPort { get; set; }

        [JsonProperty(PropertyName = "maxBatchSize")]
        public uint MaxBatchSize { get; set; }

        public object Clone()
        {
            return new DitoConfiguration()
            {
                ServerName = this.ServerName,
                ServerPort = this.ServerPort,
                MaxBatchSize = this.MaxBatchSize
            };
        }
    }
}
