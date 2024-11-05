using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Reports
{
   public class Criteria2_DTO
    {       
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long HREAW_AwardYear { get; set; }
        public long HRMD_Id { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public long HRMDES_Id { get; set; }
        public string HRMDES_DesignationName { get; set; }
        public string IVRMMC_CountryName { get; set; }
        public string IVRMMS_Name { get; set; }
        public string studentname { get; set; }
        public string ASMAY_Year { get; set; }
        public long NoofOTCStudents { get; set; }
        public long UserId { get; set; }

        public Array yearlist { get; set; }
        public Array reportlist { get; set; }
        public Array yeardata { get; set; }
        public Array deptlist { get; set; }
        public Array desglist { get; set; }
        public Array otherstatereportlist { get; set; }
        public Array othercountrycount { get; set; }
        public Array otherstatecount { get; set; }

        public Criteria2_DTO[] selectedYear { get; set; }
        public Criteria2_DTO[] selecteddept { get; set; }
        public Criteria2_DTO[] selecteddesg { get; set; }


        public Array getinstitutioncycle { get; set; }
        public Array getinstitution { get; set; }

        public long cycleid { get; set; }
        public string cyclename { get; set; }
        public int cycleorder { get; set; }
        public long NAACSL_Id { get; set; }

        public Array getparentidzero { get; set; }
        public Array getalldata { get; set; }


        public Criteria2_DTO[] selected_Inst { get; set; }
        public string NAACSL_InstitutionTypeFlg { get; set; }
        

    }
}
