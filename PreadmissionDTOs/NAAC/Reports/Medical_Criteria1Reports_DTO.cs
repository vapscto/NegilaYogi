using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Reports
{
    public class Medical_Criteria1Reports_DTO
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


        public bool NCMCVAC141_FKFromStudents { get; set; }
        public bool NCMCVAC141_FKFromteachers { get; set; }
        public bool NCMCVAC141_FKFromemployers { get; set; }
        public bool NCMCVAC141_FKFromalumni { get; set; }
        public bool FkCollFromOtherProfs { get; set; }
        public long NCMCVAC141_year { get; set; }

        public bool NCMCVAC142_FKCollAnlInstWebsite { get; set; }
        public bool NCMCVAC142_FKCollAnlFk { get; set; }
        public bool NCMCVAC142_FKCollanalysed { get; set; }
        public bool NCMCVAC142_FKcollected { get; set; }
        public long NCMCVAC142_year { get; set; }

        public Medical_Criteria1Reports_DTO[] selected_Inst { get; set; }
    }
}
