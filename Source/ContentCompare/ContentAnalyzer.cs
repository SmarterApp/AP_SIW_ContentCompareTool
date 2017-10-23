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
            WriteCsv("MatchingItemsDiff.csv", content.Compare());
            WriteCsv("NewItems.csv", content.GetNewItems());
            WriteCsv("MissingSiwReqs.csv", content.GetItemsMissingSiwRequirements());
        }

        private void WriteCsv(string fileName, IEnumerable collection)
        {
            string path = Path.Combine(
                Directory.GetCurrentDirectory(), 
                configurationProvider.Configuration["AppSettings:OutputDirectory"], 
                fileName);
            var writer = new StreamWriter(path);
            var csv = new CsvWriter(writer);

            csv.WriteRecords(collection);
            writer.Close();
        }
    }
}
