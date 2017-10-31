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

            var contentArgs = ReadArguments(args);
            ConfigurationProvider config = new ConfigurationProvider(loggerFactory, contentArgs.Item1, contentArgs.Item2);
            config.DownloadConfigFiles();

            ContentAnalyzer analyzer = new ContentAnalyzer(loggerFactory, config);
            analyzer.Analyze();
            Console.Write("\n\nPress any key to close...");
            Console.ReadKey();
        }

        static Tuple<string, string> ReadArguments(string[] args)
        {
            string oldContent = null;
            string newContent = null;
            for (int i = 0; i < args.Length - 1; i++)
            {
                if (args[i].StartsWith("-o"))
                {
                    oldContent = args[i + 1];
                }
                else if (args[i].StartsWith("-n"))
                {
                    newContent = args[i + 1];
                }
            }
            Console.WriteLine($"oldContent: {oldContent} newContent: {newContent}");
            return Tuple.Create(oldContent, newContent);
            
        }
    }
}
