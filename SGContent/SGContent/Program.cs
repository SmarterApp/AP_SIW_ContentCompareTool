using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace SGContent
{
    public class Program
    {
        static void Main(string[] args)
        {
            ILoggerFactory loggerFactory = new LoggerFactory()
              .AddConsole(LogLevel.Debug)
              .AddDebug(LogLevel.Debug);
            ILogger logger = loggerFactory.CreateLogger<Program>();

            ConfigurationProvider config = new ConfigurationProvider(loggerFactory);
            config.DownloadConfigFiles();

            ContentAnalyzer analyzer = new ContentAnalyzer(loggerFactory, config);
            analyzer.Analyze();
        }
    }
}
