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
            logger = loggerFactory.CreateLogger<ContentAnalyzer>();
            content = new ContentCompare(config, loggerFactory);
            try 
            {
                content.LoadAllItems();
            }
            catch
            {
                string oldPath = Path.GetFullPath(config.AppSettings.SbContent.ContentRootDirectory);
                string newPath = Path.GetFullPath(config.Configuration["AppSettings:ContentCompareDirectory"]);
                logger.LogCritical($"Error loading items. Ensure that the following paths are correct: \nOld Content: {oldPath}\nNew Content: {newPath}");
                Environment.Exit(1);
            }
            
            configurationProvider = config;
            
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
