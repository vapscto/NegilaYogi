using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.University
{
   public class Naac_HSU_CR4Report_DTO
    {

        public long MI_Id { get; set; }
        public long UserId { get; set; }



        public string NAACSL_InstitutionTypeFlg { get; set; }
        public Array getinstitutioncycle { get; set; }
        public Array yearlist { get; set; }
        public Array yearlist1 { get; set; }
        public Array govtsclist { get; set; }
        public Array govtsclistfiles { get; set; }
        public string ASMAY_Year { get; set; }

        public Array getinstitution { get; set; }

        public Naac_HSU_CR4Report_DTO[] selected_Inst { get; set; }
        public long cycleid { get; set; }
        public Array reportlist { get; set; }
        public Array reportlist2 { get; set; }
    }
}
