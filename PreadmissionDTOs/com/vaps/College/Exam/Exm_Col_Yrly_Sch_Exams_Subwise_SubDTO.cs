using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Exam
{
   public class Exm_Col_Yrly_Sch_Exams_Subwise_SubDTO : CommonParamDTO
    {
        public int ECYSESSS_Id { get; set; }
        public int ECYSES_Id { get; set; }
        public int EMSS_Id { get; set; }
        public int EMSE_Id { get; set; }
        public int EMGR_Id { get; set; }
        public decimal? ECYSESSS_MaxMarks { get; set; }
        public decimal? ECYSESSS_MinMarks { get; set; }
        public bool ECYSESSS_ExemptedFlg { get; set; }
        public decimal? ECYSESSS_ExemptedPer { get; set; }
        public int ECYSESSS_SubSubjectOrder { get; set; }
        public bool ECYSESSS_ActiveFlg { get; set; }

    }
}
