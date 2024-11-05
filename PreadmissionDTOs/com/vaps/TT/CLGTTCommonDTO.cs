using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.TT
{
    public class CLGTTCommonDTO
    {
        public long TTMD_Id { get; set; }

        public long MI_Id { get; set; }
       // public long ASMCL_Id { get; set; }
        public string TTMD_DayName { get; set; }
        public string TTMD_DayCode { get; set; }
        public bool TTMD_ActiveFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
     
        public string AMB_BranchName { get; set; }
        public string returnduplicatestatus { get; set; }

        public bool returnval { get; set; }
        public CLGTTCommonDTO[] crids { get; set; }
        public CLGTTCommonDTO[] brnchds { get; set; }
        public Array Daylist { get; set; }
        public Array categorylist { get; set; }
        public Array yearlist { get; set; }
        public Array courselist { get; set; }
        public Array Daydetailedit { get; set; }
        public Array semisterlist { get; set; }

        public Array Daylistedit { get; set; }
        public Array stafflist { get; set; }
        public Array subjectlist { get; set; }
        public Array branchlist { get; set; }
        public Array sectionlist { get; set; }
        public Array daydropdown { get; set; }
        public Array Daylistdetail { get; set; }
        public TT_Master_DayDTO[] temp_day_Array { get; set; }
        public TT_Master_DayDTO[] Temp_class_Array { get; set; }
       
        public Array Daylist_class { get; set; }
        public int day_count { get; set; }

        public long AMSE_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long TTMDC_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public bool TTMDC_ActiveFlag { get; set; }
        public string academicyr { get; set; }
        public string classname { get; set; }
        public string catgname { get; set; }
        public string staffName { get; set; }
        public string TTMSAB_Abbreviation { get; set; }
        public long TTMC_Id { get; set; }
        public long ISMS_Id { get; set; }
    }
}
