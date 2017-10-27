using SmarterBalanced.SampleItems.Dal.Providers.Models;
using SmarterBalanced.SampleItems.Dal.Xml.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContentCompare.Models
{
    public class ItemDigestScoring
    {
        public ItemDigest ItemDigest { get; }
        public SampleItemScoring SampleItemScoring { get; }
        public StandardIdentifier StandardIdentifier { get; }
        public GradeLevels Grade { get; set; }

        public ItemDigestScoring(ItemDigest digest, SampleItemScoring scoring, StandardIdentifier identifier)
        {
            ItemDigest = digest;
            SampleItemScoring = scoring;
            StandardIdentifier = identifier;
            Grade = GradeLevelsUtils.FromString(digest.GradeCode);
        }

        public string FormatPublications()
        {
            return String.Join(", ", ItemDigest.StandardPublications.Select(sp => sp.PrimaryStandard));
        }

        public bool HasRemovedRubrics()
        {
            bool hadRubrics = ItemDigest.Contents
                .Any(c => c.RubricList?.Rubrics.Any() == true);

            bool hasProperRubrics = SampleItemScoring.Rubrics.Any();

            return hadRubrics && !hasProperRubrics;
        }
    }
}
