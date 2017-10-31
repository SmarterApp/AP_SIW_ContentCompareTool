using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using SmarterBalanced.SampleItems.Dal.Translations;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCompare.Models
{
    public class ItemPublicationPrintout
    {
        public string ItemId { get; set; }
        public GradeLevels Grade { get; set; }
        public string Subject { get; set; }
        public string OldClaimCode { get; set; }

        public string OldTargetCode { get; set; }
        public string OldTargetDesc { get; set; }

        public string InteractionCode { get; set; }
        public string StandardPubs { get; set; }

        public ItemPublicationPrintout(ItemDigestScoring itemDigestScoring, SampleItem sampleItem)
        {
            var digest = itemDigestScoring.ItemDigest;
            ItemId = digest.ToString();
            Grade = itemDigestScoring.Grade;
            Subject = digest.SubjectCode;
            OldClaimCode = sampleItem.Claim?.ClaimNumber;
            OldTargetCode = sampleItem.CoreStandards?.Target?.IdLabel;
            OldTargetDesc = sampleItem.CoreStandards?.Target?.Descripton;
            InteractionCode = digest.InteractionTypeCode;
            StandardPubs = itemDigestScoring.FormatPublications();
        }
    }
}
