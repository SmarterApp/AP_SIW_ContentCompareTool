using Microsoft.Extensions.Logging;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using SmarterBalanced.SampleItems.Dal.Providers;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace SGContent
{
    class ContentCompare
    {
        ImmutableArray<ItemDigest> NewDigests;
        ImmutableArray<SampleItem> OldSampleItems;

        public ContentCompare(string newItemsDirectory)
        {
            ILoggerFactory loggerFactory = new LoggerFactory()
                .AddConsole()
                .AddDebug();
            ILogger logger = loggerFactory.CreateLogger<Program>();
            NewDigests = SampleItemsProvider.LoadItemDigests(newItemsDirectory).Result;
            var context = SampleItemsProvider.LoadContext(AppSettingsProvider.Instance.AppSettings, logger);
            OldSampleItems = context.SampleItems;
        }

        public IEnumerable<Comparison> Compare()
        {

        }
    }
}
