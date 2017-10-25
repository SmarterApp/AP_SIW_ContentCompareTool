using CsvHelper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
            configurationProvider = new ConfigurationProvider(loggerFactory);
            logger = loggerFactory.CreateLogger<ContentAnalyzer>();
        }

        public void Analyze()
        {
            WriteCsv("MatchingItemsDiff.csv", content.CompareOldAndNew());
            WriteCsv("NewItems.csv", content.GetNewItems());
            WriteCsv("MissingSiwReqs.csv", content.GetItemsMissingSiwRequirements());
            WriteCsv("MissingScoring.csv", content.GetItemsWithoutScoring());
            WriteCsv("ScoreInfoDiff.csv", content.CompareScoreInfo());
            WriteCsv("MissingPublications.csv", content.GetMissingPublications());
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
