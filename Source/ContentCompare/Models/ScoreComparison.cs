using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContentCompare.Models
{
    public class ScoreComparison
    {
        public int? OldRubricsCount { get; set; }
        public int? NewRubricsCount { get; set; }
        public int? OldEntriesCount { get; set; }
        public int? NewEntriesCount { get; set; }
        public int? OldSamplesCount { get; set; }
        public int? NewSamplesCount { get; set; }
        public int? OldScoreOptionsCount { get; set; }
        public int? NewScoreOptionsCount { get; set; }

        public ScoreComparison(SampleItemScoring oldScoring, SampleItemScoring newScoring)
        {
            OldRubricsCount = oldScoring?.Rubrics.Count();
            NewRubricsCount = newScoring?.Rubrics.Count();
            OldEntriesCount = oldScoring?.Rubrics.SelectMany(r => r.RubricEntries).Count();
            NewEntriesCount = newScoring?.Rubrics.SelectMany(r => r.RubricEntries).Count();
            OldSamplesCount = oldScoring?.Rubrics.SelectMany(r => r.Samples).Count();
            NewSamplesCount = newScoring?.Rubrics.SelectMany(r => r.Samples).Count();
            OldScoreOptionsCount = oldScoring?.ScoringOptions.Length;
            NewScoreOptionsCount = newScoring?.ScoringOptions.Length;
        }

        public bool Equal
        {
            get
            {
                if (NewRubricsCount != OldRubricsCount) return false;
                if (NewEntriesCount != OldEntriesCount) return false;
                if (NewSamplesCount != OldSamplesCount) return false;
                if (NewScoreOptionsCount != OldScoreOptionsCount) return false;
                return true;
            }
        }
    }
}
