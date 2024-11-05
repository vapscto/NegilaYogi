using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Medical
{
   public class HSU_MasterCR2_DTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
        public string ASMAY_Year { get; set; }

        public long NCHSUSLL221_Id { get; set; }
        public long NCHSUSLL221_Year { get; set; }
        public bool NCHSUSLL221_MsCrRegSlowPerformersFlag { get; set; }
        public bool NCHSUSLL221_MsCrRegAdLearnersFlag { get; set; }
        public bool NCHSUSLL221_SplProgSlowAdLearnersFlag { get; set; }
        public bool NCHSUSLL221_ProtocolsmesAchievementsFlag { get; set; }


        public long NCHSUCS232_Id { get; set; }
        public long NCHSUCS232_Year { get; set; }
        public bool NCHSUCS232_CsTrclinicalskillsRelevantFlag { get; set; }
        public bool NCHSUCS232_PatientSimulatorsSimulationbasedFlag { get; set; }
        public bool NCHSUCS232_StProgConductedSssessmentStudentsFlag { get; set; }
        public bool NCHSUCS232_TrProgConForCsSblearningFlag { get; set; }


        public long NCHSUEM255_Id { get; set; }
        public long NCHSUEM255_Year { get; set; }
        public bool NCHSUEM255_AnDivImpEMFlag { get; set; }
        public bool NCHSUEM255_StuRegHtIssueProcessingFlag { get; set; }
        public bool NCHSUEM255_StuRegResultProcFlag { get; set; }
        public bool NCHSUEM255_ResultProcAtdFlag { get; set; }
        public bool NCHSUEM255_ManualMethodologyFlag { get; set; }

        public bool duplicate { get; set; }
        public bool returnval { get; set; }

        

        public Array institutionlist { get; set; }
        public Array yearlist { get; set; }
        public Array alldata221 { get; set; }
        public Array alldata232 { get; set; }
        public Array alldata255 { get; set; }
    }
}
