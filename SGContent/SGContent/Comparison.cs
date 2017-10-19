using SmarterBalanced.SampleItems.Dal.Providers.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SGContent
{
    class Comparison
    {
        public int BankKey { get; set; }
        public int ItemKey { get; set; }
        public GradeLevels OldGrade { get; set; }
        public GradeLevels NewGrade { get; set; }
        public string OldSubject { get; set; }
        public string NewSubject { get; set; }
        public string OldClaim { get; set; }
        public string NewClaim { get; set; }
        public string OldTarget { get; set; }
        public string NewTarget { get; set; }
        public string OldInteractionCode { get; set; }
        public string NewInteractionCode { get; set; }
        public bool? OldAslSupported { get; set; }
        public bool? NewAslSupported { get; set; }
        public int? OldStimulus { get; set; }
        public int? NewStimulus { get; set; }
        public string OldDOK { get; set; }
        public string NewDOK { get; set; }
        public string OldCoreStandard { get; set; }
        public string NewCoreStandard { get; set; }
        public string NewStandardPublications { get; set; }

        public bool Equal
        {
            get
            {
                if (NewGrade != OldGrade) return false;
                if (NewSubject != OldSubject) return false;
                if (NewClaim != OldClaim) return false;
                if (NewTarget != OldTarget) return false;
                if (NewInteractionCode != OldInteractionCode) return false;
                if (NewAslSupported != OldAslSupported) return false;
                if (NewStimulus != OldStimulus) return false;
                if (NewDOK != OldDOK) return false;
                if (NewCoreStandard != OldCoreStandard) return false;
                return true;
            }
        }
    }
}
