using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentCompare.Models
{
    public class ItemDigestScoring
    {
        public ItemDigest ItemDigest { get; }
        public SampleItemScoring SampleItemScoring { get; }

        public ItemDigestScoring(ItemDigest digest, SampleItemScoring scoring)
        {
            ItemDigest = digest;
            SampleItemScoring = scoring;
        }
    }
}
