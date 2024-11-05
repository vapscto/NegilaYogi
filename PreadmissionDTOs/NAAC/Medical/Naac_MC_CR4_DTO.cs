using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Medical
{
   public class Naac_MC_CR4_DTO
    {
        public long MI_Id { get; set; }
        public long NCAC414BD_Id { get; set; }
        public long NCAC414BDF_Id { get; set; }
        public string NCAC414BDF_FileName { get; set; }
        public string NCAC414BDF_Filedesc { get; set; }
        public string NCAC414BDF_FilePath { get; set; }
        public long ASMAY_Id { get; set; }
        public string ASMAY_Year { get; set; }
        public decimal NCAC414BD_BudgetINfDev { get; set; }
        public decimal NCAC414BD_BudgetINfAugn { get; set; }
        public long NCAC414BD_AllotYear { get; set; }
        public long UserId { get; set; }
        public string NAACSL_InstitutionTypeFlg { get; set; }
        public Array getinstitutioncycle { get; set; }
        public Array yearlist { get; set; }
        public Array yearlist1 { get; set; }
       
        public Array getinstitution { get; set; }
     
        public Naac_MC_CR4_DTO[] selected_Inst { get; set; }
        public long cycleid { get; set; }
        public Array reportlist { get; set; }
        public Array reportlist2 { get; set; }
    }
}
