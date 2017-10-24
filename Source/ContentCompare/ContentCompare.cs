using ContentCompare.Models;
using Microsoft.Extensions.Logging;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using SmarterBalanced.SampleItems.Dal.Providers;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using SmarterBalanced.SampleItems.Dal.Translations;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace SGContent
{
    public class ContentCompare
    {
        private readonly ImmutableArray<ItemDigest> newDigests;
        private readonly ImmutableArray<SampleItem> oldSampleItems;
        private readonly ConfigurationProvider config;

        public ContentCompare(ConfigurationProvider config, ILoggerFactory loggerFactory)
        {
            ILogger logger = loggerFactory.CreateLogger<ContentCompare>();
            newDigests = SampleItemsProvider.LoadItemDigests(config.Configuration["AppSettings:ContentCompareDirectory"]).Result;
            var context = SampleItemsProvider.LoadContext(config.AppSettings, logger);
            oldSampleItems = context.SampleItems;
            this.config = config;
        }

        public IEnumerable<Comparison> CompareOldAndNew()
        {
            var matchingItems = newDigests.Join(oldSampleItems,
                digest => digest.ItemKey,
                sampleItem => sampleItem.ItemKey,
                (digest, sampleItem) =>
                {
                    return new Comparison(sampleItem, digest, config.AppSettings.SbContent.SupportedPublications);
                });

            var differentItems = matchingItems.Where(c => !c.Equal);
            return differentItems;
        }

        public IEnumerable<ItemPrintout> GetNewItems()
        {
            var newItems = newDigests.Where(d => oldSampleItems.FirstOrDefault(si => si.ItemKey == d.ItemKey) == null)
                .Select(digest => new ItemPrintout(digest, config.AppSettings));
            return newItems;
        }

        public IEnumerable<ItemPrintout> GetItemsWithoutScoring()
        {
            var noScoring = newDigests
                .Where(digest => SampleItemTranslation.GetRubrics(digest, config.AppSettings).Length == 0)
                .Select(digest => new ItemPrintout(digest, config.AppSettings));
            return noScoring;
        }

        public IEnumerable<ItemPrintout> GetItemsMissingSiwRequirements()
        {
            var missingReqs = newDigests
                .Where(digest =>
                {
                    if (digest.StandardPublications == null || digest.StandardPublications.Count == 0) return true;
                    if (String.IsNullOrEmpty(digest.GradeCode)) return true;
                    if (String.IsNullOrEmpty(digest.InteractionTypeCode)) return true;
                    if (String.IsNullOrEmpty(digest.SubjectCode)) return true;
                    return false;
                })
                .Select(digest => new ItemPrintout(digest, config.AppSettings));
            return missingReqs;
        }

        public IEnumerable<ScoreComparison> CompareScoreInfo()
        {
            var matchingItems = newDigests.Join(oldSampleItems,
                digest => digest.ItemKey,
                sampleItem => sampleItem.ItemKey,
                (digest, sampleItem) => new { Digest = digest, SampleItem = sampleItem });

            var scoreComparison = matchingItems.Select(match =>
            {
                var digestRubrics = SampleItemTranslation.GetRubrics(match.Digest, config.AppSettings);
                var sampleItemRubrics = match.SampleItem.Rubrics;

                return new ScoreComparison(sampleItemRubrics, digestRubrics);
            });
            return scoreComparison.Where(sc => !sc.Equal);
        }
    }
}
