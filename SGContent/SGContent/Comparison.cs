using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SGContent
{
    class Comparison
    {
        bool Different { get; set; }
        int BankKey { get; set; }
        int ItemKey { get; set; }
        GradeLevels OldGrade { get; set; }
        GradeLevels NewGrade { get; set; }
        string OldSubject { get; set; }
        string NewSubject { get; set; }
        string OldClaim { get; set; }
        string NewClaim { get; set; }
        string OldTarget { get; set; }
        //OldTarget = sampleItem.CoreStandards?.Target?.Id,
        //                NewTarget = digestIdentifier?.ToTargetId(),
        //                OldInteractionCode = sampleItem.InteractionType?.Code,
        //                NewInteractionCode = digest.InteractionTypeCode,
        //                OldAslSupported = sampleItem.AslSupported,
        //                NewAslSupported = oldAslSupported,
        //                OldStimulus = sampleItem.AssociatedStimulus,
        //                NewStimulus = digest.AssociatedStimulus,
        //                OldDOK = sampleItem.DepthOfKnowledge,
        //                NewDOK = digest.DepthOfKnowledge,
        //                NewStandardPublications = standardPubs
    }
}
