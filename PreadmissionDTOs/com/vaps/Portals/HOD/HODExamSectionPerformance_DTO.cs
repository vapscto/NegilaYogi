using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Portals.HOD
{
  public class HODExamSectionPerformance_DTO:CommonParamDTO
    {
        public long MI_Id { get; set; }
        public string ASMAY_Year { get; set; }
        public long ASMAY_Id { get; set; }
        public int EMCA_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public int EME_Id { get; set; }
        public string EME_ExamName { get; set; }
        public string EMCA_CategoryName { get; set; }
        public long ISMS_Id { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string ASMC_SectionName { get; set; }
        public decimal? ESTMPS_SectionAverage { get; set; }
        public long user_id { get; set; }



        public Array yearlist { get; set; }
        public Array classlist { get; set; }
        public Array exmstdlist { get; set; }
        public Array fillcategory { get; set; }
        public Array subjlist { get; set; }
        public Array seclist { get; set; }


    }
}
