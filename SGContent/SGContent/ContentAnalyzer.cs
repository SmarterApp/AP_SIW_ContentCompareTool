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

        public ContentAnalyzer(ILoggerFactory loggerFactory, ConfigurationProvider config) 
        {
            content = new ContentCompare(config, loggerFactory);
            configurationProvider = new ConfigurationProvider(loggerFactory);
        }

        public void Analyze()
        {
            string dirPath = Path.Combine(Directory.GetCurrentDirectory(), configurationProvider.Configuration["AppSettings:OutputDirectory"]);

            string matchingItemsPath = Path.Combine(dirPath, "MatchingItemsDiff.csv");
            WriteCsv(matchingItemsPath, content.Compare());

            string newItemsPath = Path.Combine(dirPath, "NewItems.csv");
            WriteCsv(newItemsPath, content.GetNewItems());
        }

        private void WriteCsv(string fileName, IEnumerable collection)
        {
            var writer = new StreamWriter(fileName);
            var csv = new CsvWriter(writer);

            csv.WriteRecords(collection);
            writer.Close();
        }
    }
}
