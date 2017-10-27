using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using SmarterBalanced.SampleItems.Dal.Translations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContentCompare.Models
{
    public class ItemScoringPrintout
    {
        public string ItemId { get; set; }
        public GradeLevels Grade { get; set; }
        public string Subject { get; set; }
        public string Claim { get; set; }
        public string Target { get; set; }
        public string InteractionCode { get; set; }
        public bool RemovedRubric { get; set; }

        public ItemScoringPrintout(ItemDigestScoring itemDigestScoring, AppSettings settings) 
        {
            var digest = itemDigestScoring.ItemDigest;
            var supportedPubs = settings.SbContent.SupportedPublications;
            ItemId = digest.ToString();
            Grade = itemDigestScoring.Grade;
            Subject = digest.SubjectCode;
            Claim = itemDigestScoring.StandardIdentifier?.ToClaimId();
            Target = itemDigestScoring.StandardIdentifier?.ToTargetId();
            InteractionCode = digest.InteractionTypeCode;
            RemovedRubric = itemDigestScoring.HasRemovedRubrics();
        }

    }
}
