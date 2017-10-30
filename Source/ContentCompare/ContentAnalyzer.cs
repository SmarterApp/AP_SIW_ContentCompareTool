using CsvHelper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SGContent
{
    public class ContentAnalyzer
    {
        private readonly ConfigurationProvider configurationProvider;
        private readonly ContentCompare content;
        private readonly ILogger logger;

        public ContentAnalyzer(ILoggerFactory loggerFactory, ConfigurationProvider config)
        {
            content = new ContentCompare(config, loggerFactory);
            content.LoadAllItems();
            configurationProvider = config;
            logger = loggerFactory.CreateLogger<ContentAnalyzer>();
        }

        public void Analyze()
        {
            Task.WaitAll(
                WriteCsvAsync("MatchingItemsDiff.csv", content.CompareOldAndNew()),
                WriteCsvAsync("NewItems.csv", content.GetNewItems()),
                WriteCsvAsync("MissingSiwReqs.csv", content.GetItemsMissingSiwRequirements()),
                WriteCsvAsync("MissingScoring.csv", content.GetItemsWithoutScoring()),
                WriteCsvAsync("ScoreInfoDiff.csv", content.CompareScoreInfo()),
                WriteCsvAsync("MissingPublications.csv", content.GetMissingPublications()));
        }

        private async Task WriteCsvAsync(string fileName, IEnumerable collection)
        {
            await Task.Run(() => WriteCsv(fileName, collection));
        } 

        private void WriteCsv(string fileName, IEnumerable collection)
        {
            string path = Path.Combine(
                Directory.GetCurrentDirectory(), 
                configurationProvider.Configuration["AppSettings:OutputDirectory"], 
                fileName);

            Directory.CreateDirectory(Path.GetDirectoryName(path));
            if (File.Exists(path))
            {
                logger.LogInformation($"File exists, removing {fileName}");
                File.Delete(path);
            }

            var writer = new StreamWriter(path);
            var csv = new CsvWriter(writer);

            csv.WriteRecords(collection);
            writer.Close();
        }
    }
}
