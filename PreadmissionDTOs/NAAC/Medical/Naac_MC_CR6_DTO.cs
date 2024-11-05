using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Medical
{
   public class Naac_MC_CR6_DTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public Array reportlist2 { get; set; }
        public Array reportlist { get; set; }
        public long cycleid { get; set; }
        public Naac_MC_CR6_DTO[] selected_Inst { get; set; }
        public Array getinstitutioncycle { get; set; }
        public Array getinstitution { get; set; }
        public Array yearlist { get; set; }
        public string NAACSL_InstitutionTypeFlg { get; set; }
    }
}
