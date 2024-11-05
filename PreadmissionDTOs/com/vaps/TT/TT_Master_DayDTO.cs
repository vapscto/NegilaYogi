using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.TT
{
    public class TT_Master_DayDTO
    {
        public long TTMD_Id { get; set; }

        public long MI_Id { get; set; }
       // public long ASMCL_Id { get; set; }
        public string TTMD_DayName { get; set; }
        public string TTMD_DayCode { get; set; }
        public bool TTMD_ActiveFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    
        public string returnduplicatestatus { get; set; }

        public bool returnval { get; set; }
        
        public Array periodlist { get; set; }
        public Array Daylist { get; set; }
        public Array dayorderlist { get; set; }
        public Array Daydetailedit { get; set; }

        public Array Daylistedit { get; set; }
        public Array Daylisttwo { get; set; }
        public Array Daylistdetail { get; set; }
        public Array yearlist { get; set; }
        public TT_Master_DayDTO[] temp_day_Array { get; set; }
        public TT_Master_DayDTO[] Temp_class_Array { get; set; }
       
        public Array Daylist_class { get; set; }
        public int day_count { get; set; }

        public long TTMDC_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public bool TTMDC_ActiveFlag { get; set; }
        public string academicyr { get; set; }
        public string classname { get; set; }
        public string catgname { get; set; }
        public long TTMC_Id { get; set; }
        public temp_masterday[] ordeidss { get; set; }
        public allocateperidids[] alocateids { get; set; }
    }

    public class temp_masterday
    {
        public long TTMD_Id { get; set; }
        public long Order_Id { get; set; }
    }
    public class allocateperidids
    {
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long TTAP_NoOfPeriods { get; set; }
    }
}
