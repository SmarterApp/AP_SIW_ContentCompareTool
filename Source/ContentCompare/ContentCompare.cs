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
        private readonly ImmutableArray<ItemDigestScoring> newDigestsScoring;
        private readonly ImmutableArray<SampleItem> oldSampleItems;
        private readonly ConfigurationProvider config;
        private readonly ILogger logger;

        public ContentCompare(ConfigurationProvider config, ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<ContentCompare>();
            this.config = config;
            var newItemDigests = SampleItemsProvider
                .LoadItemDigests(config.Configuration["AppSettings:ContentCompareDirectory"]).Result;
            var context = SampleItemsProvider.LoadContext(config.AppSettings, logger);
            oldSampleItems = context.SampleItems;
            newDigestsScoring = newItemDigests.Select(digest =>
            {
                var scoring = SampleItemsScoringTranslation.ToSampleItemsScore(digest, config.AppSettings, context.InteractionTypes);
                return new ItemDigestScoring(digest, scoring);
            }).ToImmutableArray();
        }

        public IEnumerable<Comparison> CompareOldAndNew()
        {
            var matchingItems = newDigestsScoring.Join(oldSampleItems,
                digest => digest.ItemDigest.ItemKey,
                sampleItem => sampleItem.ItemKey,
                (digest, sampleItem) =>
                {
                    return new Comparison(
                        sampleItem, 
                        digest, 
                        config.AppSettings.SbContent.SupportedPublications);
                });

            var differentItems = matchingItems.Where(c => !c.Equal);
            return differentItems;
        }

        public IEnumerable<ItemPrintout> GetNewItems()
        {
            var newItems = newDigestsScoring
                .Where(d => oldSampleItems.FirstOrDefault(si => si.ItemKey == d.ItemDigest.ItemKey) == null)
                .Select(digest => new ItemPrintout(digest, config.AppSettings));
            return newItems;
        }

        //TODO add options
        public IEnumerable<ItemPrintout> GetItemsWithoutScoring()
        {
            var noScoring = newDigestsScoring
                .Where(digest => !digest.SampleItemScoring.Rubrics.Any())
                .Select(digest => new ItemPrintout(digest, config.AppSettings));
            return noScoring;
        }

        public IEnumerable<ItemPrintout> GetItemsMissingSiwRequirements()
        {
            var missingReqs = newDigestsScoring
                .Where(digestScoring =>
                {
                    var digest = digestScoring.ItemDigest;
                    if (digest.StandardPublications == null || digest.StandardPublications.Count == 0)
                        return true;
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
            var matchingItems = newDigestsScoring.Join(oldSampleItems,
                digest => digest.ItemDigest.ItemKey,
                sampleItem => sampleItem.ItemKey,
                (digest, sampleItem) => new { Digest = digest, SampleItem = sampleItem });

            var scoreComparison = matchingItems.Select(match =>
            {
                var digestRubrics = match.Digest.SampleItemScoring?.Rubrics;
                var sampleItemRubrics = match.SampleItem.SampleItemScoring?.Rubrics;

                return new ScoreComparison(sampleItemRubrics, digestRubrics);
            });

            return scoreComparison.Where(sc => !sc.Equal);
        }
    }
}
