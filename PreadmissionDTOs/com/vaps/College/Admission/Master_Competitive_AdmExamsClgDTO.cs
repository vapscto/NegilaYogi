using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
   public class Master_Competitive_AdmExamsClgDTO
    {
        public long AMCEXM_Id { get; set; }
        public long MI_Id { get; set; }
        public string AMCEXM_CompetitiveExams { get; set; }
        public bool AMCEXM_CompulsoryFlg { get; set; }
        public bool AMCEXM_ActiveFlg { get; set; }
        public long AMCEXM_CreatedBy { get; set; }
        public long AMCEXM_UpdatedBy { get; set; }
        public DateTime AMCEXM_CreatedDate { get; set; }
        public DateTime AMCEXM_UpdatedDate { get; set; }

        public long ASMAY_Id { get; set; }

        public long ID { get; set; }

        public Array examdetailsarray { get; set; }

        public Array subdetailsarray { get; set; }
        public string returnMsg { get; set; }
        public bool returnval { get; set; }

        public Array pagesdata { get; set; }
        public Array pagesdatatwo { get; set; }
        public Array pagesdatasubedit { get; set; }


        public Array MasterExamData { get; set; }

        //subject

        public long AMCEXMSUB_Id { get; set; }
        public string AMCEXMSUB_SubjectName { get; set; }
        public bool AMCEXMSUB_ActiveFlg { get; set; }
        public DateTime AMCEXMSUB_CreatedDate { get; set; }
        public DateTime AMCEXMSUB_UpdatedDate { get; set; }

        public decimal? AMCEXMSUB_MaxMarks { get; set; }

    }
}
