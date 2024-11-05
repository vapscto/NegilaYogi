using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Reports
{
   public class Medical_Criteria3Reports_DTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long cycleid { get; set; }
        public string NAACSL_InstitutionTypeFlg { get; set; }

        public Array getinstitutioncycle { get; set; }
        public Array getinstitution { get; set; }
        public Array yearlist { get; set; }
        public Array reportlist { get; set; }
        public Array reportlist2 { get; set; }
        public string ASMAY_Year { get; set; }

        public Medical_Criteria3Reports_DTO[] selected_Inst { get; set; }


    }
}
