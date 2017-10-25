using SmarterBalanced.SampleItems.Dal.Providers.Models;
using SmarterBalanced.SampleItems.Dal.Xml.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCompare.Models
{
    public class ItemDigestScoring
    {
        public ItemDigest ItemDigest { get; }
        public SampleItemScoring SampleItemScoring { get; }
        public StandardIdentifier StandardIdentifier { get; }

        public ItemDigestScoring(ItemDigest digest, SampleItemScoring scoring, StandardIdentifier identifier)
        {
            ItemDigest = digest;
            SampleItemScoring = scoring;
            StandardIdentifier = identifier;
        }
    }
}
