using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContentCompare.Models
{
    public class ScoreComparison
    {
        public int OldRubricsCount { get; set; }
        public int NewRubricsCount { get; set; }
        public int OldEntriesCount { get; set; }
        public int NewEntriesCount { get; set; }
        public int OldSamplesCount { get; set; }
        public int NewSamplesCount { get; set; }

        public ScoreComparison(IEnumerable<Rubric> oldRubrics, IEnumerable<Rubric> newRubrics)
        {
            OldRubricsCount = oldRubrics.Count();
            NewRubricsCount = newRubrics.Count();
            OldEntriesCount = oldRubrics.SelectMany(r => r.RubricEntries).Count();
            NewEntriesCount = newRubrics.SelectMany(r => r.RubricEntries).Count();
            OldSamplesCount = oldRubrics.SelectMany(r => r.Samples).Count();
            NewSamplesCount = newRubrics.SelectMany(r => r.Samples).Count();
        }

        public bool Equal { get
            {
                if (NewRubricsCount != OldRubricsCount) return false;
                if (NewEntriesCount != OldEntriesCount) return false;
                if (NewSamplesCount != OldSamplesCount) return false;
                return true;
            }
        }
    }
}
