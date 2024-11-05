using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.University
{
   public class HSU_341_EthicsDTO
    {
        public long NCMC331ES_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long NCMC331ES_Year { get; set; }
        public bool NCMC331ES_InstEthicsCommitteeOverseesimpReshProjFlag { get; set; }
        public bool NCMC331ES_AllTeshProjStuProjEhicsCommitteeClearanceFlag { get; set; }
        public bool NCMC331ES_InstPlagiarismSoftInstPolicyFlag { get; set; }
        public bool NCMC331ES_NormsGdsReshEthicsGuidelinesFollowedbitlag { get; set; }
        public long NCMC331ES_CreatedBy { get; set; }
        public long NCMC331ES_UpdatedBy { get; set; }
        public DateTime? NCMC331ES_CreatedDate { get; set; }
        public DateTime? NCMC331ES_UpdatedDate { get; set; }
        public bool NCMC331ES_ActiveFlag { get; set; }

        public long ASMAY_Id { get; set; }
        public Array allacademicyear { get; set; }
        public Array alldata1 { get; set; }
        public Array institutionlist { get; set; }
        public string ASMAY_Year { get; set; }
        public bool duplicate { get; set; }
        public string msg { get; set; }
        public bool returnval { get; set; }
        public Array editlist { get; set; }
        public Array yearlist { get; set; }


    }
}
