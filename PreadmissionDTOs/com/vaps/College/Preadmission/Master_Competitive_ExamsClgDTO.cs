using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Preadmission
{
   public class Master_Competitive_ExamsClgDTO
    {
        public long PAMCEXM_Id { get; set; }
        public long MI_Id { get; set; }
        public string PAMCEXM_CompetitiveExams { get; set; }
        public bool PAMCEXM_CompulsoryFlg { get; set; }
        public bool PAMCEXM_ActiveFlg { get; set; }
        public long PAMCEXM_CreatedBy { get; set; }
        public long PAMCEXM_UpdatedBy { get; set; }
        public DateTime PAMCEXM_CreatedDate { get; set; }
        public DateTime PAMCEXM_UpdatedDate { get; set; }

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

        public long PAMCEXMSUB_Id { get; set; }
        public string PAMCEXMSUB_SubjectName { get; set; }
        public bool PAMCEXMSUB_ActiveFlg { get; set; }
        public DateTime PAMCEXMSUB_CreatedDate { get; set; }
        public DateTime PAMCEXMSUB_UpdatedDate { get; set; }

        public decimal? PAMCEXMSUB_MaxMarks { get; set; }
    
    }
}
