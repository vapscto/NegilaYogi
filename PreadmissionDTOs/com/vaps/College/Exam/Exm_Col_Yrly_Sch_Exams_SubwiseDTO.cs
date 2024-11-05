using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Exam
{
  public  class Exm_Col_Yrly_Sch_Exams_SubwiseDTO
    {
        public long ECYSES_Id { get; set; }
        public long ECYSE_Id { get; set; }
        public long ISMS_Id { get; set; }
        public int EMGR_Id { get; set; }
        public decimal ECYSES_MarksEntryMax { get; set; }
        public decimal ECYSES_MaxMarks { get; set; }
        public decimal ECYSES_MinMarks { get; set; }
        public bool ECYSES_SubExamFlg { get; set; }
        public bool ECYSES_SubSubjectFlg { get; set; }
        public string ECYSES_MarksGradeEntryFlg { get; set; }
        public bool ECYSES_MarksDisplayFlg { get; set; }
        public bool ECYSES_GradeDisplayFlg { get; set; }
        public bool ECYSES_AplResultFlg { get; set; }
        public bool ECYSES_SubjectOrder { get; set; }
        public bool ECYSES_ActiveFlg { get; set; }

    }
}
