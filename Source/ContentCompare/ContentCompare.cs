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
using System.Threading.Tasks;

namespace SGContent
{
    public class ContentCompare
    {
        private readonly ConfigurationProvider config;
        private readonly ILogger logger;

        // content package
        private ImmutableArray<ItemDigestScoring> newDigestsScoring;

        //Old content package
        private ImmutableArray<SampleItem> oldSampleItems;

        public ContentCompare(ConfigurationProvider config, ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<ContentCompare>();
            this.config = config;
        }

        public void LoadAllItems()
        {
            string newDigestsDir = config.Configuration["AppSettings:ContentCompareDirectory"];
            string[] supportedPubs = config.AppSettings.SbContent.SupportedPublications;
            ImmutableArray<ItemDigest> digests;
            SampleItemsContext context = null;

            Task.WaitAll(
                Task.Run(() => context = SampleItemsProvider.LoadContext(config.AppSettings, logger)),
                Task.Run(() => digests = SampleItemsProvider.LoadItemDigests(newDigestsDir).Result));

            newDigestsScoring = digests.Select(digest =>
            {
                var scoring = SampleItemsScoringTranslation.ToSampleItemsScore(digest, config.AppSettings, context.InteractionTypes);
                var standardIdentifier = StandardIdentifierTranslation.ToStandardIdentifier(digest, supportedPubs);
                return new ItemDigestScoring(digest, scoring, standardIdentifier);
            })
            .OrderBy(d => d.ItemDigest.SubjectCode)
            .ThenBy(d => d.Grade)
            .ThenBy(d => d.StandardIdentifier?.ToClaimId())
            .ThenBy(d => d.ItemDigest.ItemKey)
            .ToImmutableArray();
            oldSampleItems = context.SampleItems;
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

        public IEnumerable<ItemScoringPrintout> GetItemsWithoutScoring()
        {
            var noScoring = newDigestsScoring
                .Where(digest => !(digest.SampleItemScoring?.Rubrics.Any() ?? false)
                              && !(digest.SampleItemScoring?.ScoringOptions.Any() ?? false))
                .Select(digest => new ItemScoringPrintout(digest, config.AppSettings));
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
                new ScoreComparison(
                    oldScoring: match.Digest.SampleItemScoring, 
                    newScoring: match.SampleItem.SampleItemScoring, 
                    itemKey: match.Digest.ItemDigest.ItemKey, 
                    bankKey: match.Digest.ItemDigest.BankKey));

            return scoreComparison.Where(sc => !sc.Equal);
        }

        public IEnumerable<ItemPublicationPrintout> GetMissingPublications()
        {
            var missingPubs =
                from d in newDigestsScoring
                join o in oldSampleItems on d.ItemDigest.ItemKey equals o.ItemKey into ds
                from o in ds.DefaultIfEmpty()
                where String.IsNullOrEmpty(d.StandardIdentifier?.Publication)
                select new ItemPublicationPrintout(d, o); 

            return missingPubs;
        }
    }
}
