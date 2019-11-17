using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostConfig = new ConfigurationBuilder()
                .AddCommandLine(args)
                .AddJsonFile("appsettings.json")
                .Build();

            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseConfiguration(hostConfig);
                    webBuilder.UseUrls();
                    webBuilder.UseKestrel();
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}
