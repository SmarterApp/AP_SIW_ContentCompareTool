using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using SmarterBalanced.SampleItems.Dal.Providers.Models;
using SmarterBalanced.SampleItems.Dal.Translations;
using SmarterBalanced.SampleItems.Dal.Xml.Models;
using System;
using System.Linq;

namespace SGContent
{
    public class ItemPrintout
    {
        public int BankKey { get; set; }
        public int ItemKey { get; set; }
        public GradeLevels Grade { get; set; }
        public string Subject { get; set; }
        public string Claim { get; set; }
        public string Target { get; set; }
        public string InteractionCode { get; set; }
        public bool AslSupported { get; set; }
        public int? Stimulus { get; set; }
        public string DOK { get; set; }
        public string StandardPubs { get; set; }

        public ItemPrintout(ItemDigest digest, AppSettings settings)
        {
            var supportedPubs = settings.SbContent.SupportedPublications;
            StandardIdentifier digestIdentifier = StandardIdentifierTranslation.ToStandardIdentifier(digest, supportedPubs);
            BankKey = digest.BankKey;
            ItemKey = digest.ItemKey;
            Grade = GradeLevelsUtils.FromString(digest.GradeCode);
            Subject = digest.SubjectCode;
            Claim = digestIdentifier?.ToClaimId();
            Target = digestIdentifier?.ToTargetId();
            InteractionCode = digest.InteractionTypeCode;
            AslSupported = SampleItemTranslation.AslSupported(digest);
            Stimulus = digest.AssociatedStimulus;
            DOK = digest.DepthOfKnowledge;
            StandardPubs = String.Join(", ", digest.StandardPublications.Select(sp => sp.PrimaryStandard));
        }
    }
}
