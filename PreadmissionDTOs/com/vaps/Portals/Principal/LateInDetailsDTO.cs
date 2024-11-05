using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Portals.Principal
{
    public class LateInDetailsDTO : CommonParamDTO
    {
       public long MI_Id { get; set; }
       public DateTime? fromdate { get; set; }
       public Array filldepartment { get; set; }

        public Array yearlist { get; set; }


        public Array Fillstudentstrenth { get; set; }
        public long ASMAY_Id { get; set; }
        public long MIID { get; set; }
        public long classid { get; set; }
        public string empFname { get; set; }
        public string empMname { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string FOEST_IHalfLoginTime { get; set; }
        public string empLname { get; set; }
        public long asmS_Id { get; set; }
        public long asmcL_Id { get; set; }
        public Array sectionwisestrenth { get; set; }
        public Array fillsectioncount { get; set; }
        public Array classarray { get; set; }
        public Array Emp_punchDetails { get; set; }
        public string section { get; set; }
        public DateTime? FOEP_PunchDate { get; set; }

        public string FOEPD_PunchTime { get; set; }
        public string ts { get; set; }
      


    }
}


