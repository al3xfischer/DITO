using Newtonsoft.Json;
using System;

namespace Client.Models
{
    public class DitoConfiguration : ICloneable
    {
        [JsonProperty(PropertyName = "serverName")]
        public string ServerName { get; set; }

        [JsonProperty(PropertyName = "serverPort")]
        public int ServerPort { get; set; }

        [JsonProperty(PropertyName = "maxBatchSize")]
        public int MaxBatchSize { get; set; }

        [JsonProperty(PropertyName = "localServerPort")]
        public int LocalServerPort { get; set; }

        public object Clone()
        {
            return new DitoConfiguration()
            {
                ServerName = this.ServerName,
                ServerPort = this.ServerPort,
                MaxBatchSize = this.MaxBatchSize,
                LocalServerPort = this.LocalServerPort
            };
        }
    }
}
