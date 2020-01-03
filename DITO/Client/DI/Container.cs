using Client.Services.Interfaces;
using Client.Services.Provider;
using Client.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Client.DI
{
    public class Container
    {

        /// <summary>
        /// The provider.
        /// </summary>
        private static IServiceProvider provider;

        /// <summary>
        /// Initializes the <see cref="Container"/> class.
        /// </summary>
        static Container()
        {

            IServiceCollection services = new ServiceCollection();

            // ViewModels
            services.AddTransient<MainViewModel>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<BrowserViewModel>();

            // Services
            services.AddSingleton<IConfigurationService, ConfigurationService>();
            services.AddTransient<TorrentFileServiceImpl>();
            services.AddTransient<RegisterFilesServiceImpl>();
            services.AddTransient<FileRequestServiceImpl>();
            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<IClientServerService,ClientServerService>();
            services.AddSingleton<IDownloadService, DownloadService>();
            services.AddSingleton<ClientToClientService>();

            // Logging
            services.AddLogging();

            provider = services.BuildServiceProvider();

            Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "dito/logs/"));

            // Logging Configuration
            provider.GetService<ILoggerFactory>()
                .AddFile(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "dito/logs/dito.log"),minimumLevel: LogLevel.Trace, fileSizeLimitBytes: 1024 * 1024 * 3);
        }

        /// <summary>
        /// Resolves this instance.
        /// </summary>
        /// <typeparam name="TItem">The type of the item.</typeparam>
        /// <returns></returns>
        public static TItem Resolve<TItem>()
        {
            return provider.GetService<TItem>();
        }
    }
}
