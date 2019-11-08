using Client.Models;
using Client.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;

namespace Client.Services.Provider
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly ILogger<ConfigurationService> logger;

        private readonly DitoConfiguration appConfiguration;

        public ConfigurationService(ILogger<ConfigurationService> logger)
        {
            this.logger = logger;
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("config.json", optional: true, reloadOnChange: true).Build();

            this.appConfiguration = configuration.GetSection("appconfig").Get<DitoConfiguration>() ?? new DitoConfiguration();
            this.logger.LogInformation("Configuration loaded.");
        }

        public string ServerName
        {
            get => appConfiguration.ServerName;
            set => appConfiguration.ServerName = value ?? throw new ArgumentNullException(nameof(ServerName));
        }

        public uint ServerPort
        {
            get => appConfiguration.ServerPort;
            set => appConfiguration.ServerPort = value;
        }

        public uint MaxBatchSize
        {
            get => appConfiguration.MaxBatchSize;
            set => appConfiguration.ServerPort = value;
        }

        public DitoConfiguration Configuration => this.appConfiguration;

        public void Save()
        {
            this.PersistsConfiguration(this.appConfiguration);
        }

        private void PersistsConfiguration(DitoConfiguration config)
        {
            using (var fileStream =  File.Exists("config.json") ? new FileStream("config.json", FileMode.Truncate, FileAccess.Write) : File.OpenWrite("config.json"))
            using (var writer = new StreamWriter(fileStream))
            {
                var serializer = new JsonSerializer() { Formatting = Formatting.Indented };
                var globalObject = new GlobalConfiguration() { appConfig = config };
                serializer.Serialize(writer, globalObject);
            }
            this.logger.LogInformation("Configuration saved.");
        }

    }
}
