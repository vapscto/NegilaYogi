using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.TT
{
    public class CLGMasterDayDTO
    {
        public long TTMD_Id { get; set; }

        public long MI_Id { get; set; }
       // public long ASMCL_Id { get; set; }
        public string TTMD_DayName { get; set; }
        public string TTMD_DayCode { get; set; }
        public bool TTMD_ActiveFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public long Order_Id { get; set; }
        public string returnduplicatestatus { get; set; }

        public bool returnval { get; set; }
        
        public Array Daylist { get; set; }
        public Array categorylist { get; set; }
        public Array yearlist { get; set; }
        public Array courselist { get; set; }
        public Array Daydetailedit { get; set; }

        public Array Daylistedit { get; set; }
        public Array branchlist { get; set; }
        public Array daydropdown { get; set; }
        public Array daymappedlist { get; set; }
        public Array dayorderlist { get; set; }
        public CLGMasterDayDTO[] dayids { get; set; }
        public CLGMasterDayDTO[] semids { get; set; }
       
        public Array Daylist_class { get; set; }
        public int day_count { get; set; }

        public long TTMDC_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public bool TTMDC_ActiveFlag { get; set; }
        public string academicyr { get; set; }
        public string classname { get; set; }
        public string AMB_BranchName { get; set; }
        public string  AMCO_CourseName { get; set; }
        public string AMSE_SEMName { get; set; }
        public string ASMAY_Year { get; set; }
        public string catgname { get; set; }
        public string returnMsg { get; set; }
        public long TTMC_Id { get; set; }
        public temp_masterday[] ordeidss { get; set; }

    }
}
