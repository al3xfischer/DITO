using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Client.Models
{
    public class GlobalConfiguration
    {
        [JsonProperty(PropertyName = "appconfig")]
        public DitoConfiguration appConfig { get; set; }
    }
}
