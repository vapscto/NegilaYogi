using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Reports
{
   public class HSU_CR1_Report_DTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public Array getinstitutioncycle { get; set; }
        public long cycleid { get; set; }
        public string cyclename { get; set; }
        public int cycleorder { get; set; }

        public Array getparentidzero { get; set; }
        public Array reportlist { get; set; }
        public Array reportlist2 { get; set; }
        public Array getinstitution { get; set; }
        public Array yearlist { get; set; }

        public HSU_CR1_Report_DTO[] selectedYear { get; set; }
        public HSU_CR1_Report_DTO[] selected_Inst { get; set; }
        public string NAACSL_InstitutionTypeFlg { get; set; }
        public string ASMAY_Year { get; set; }
        public Array yearlist1 { get; set; }
        public Array govtsclist { get; set; }
        public Array govtsclistfiles { get; set; }
    }
}
