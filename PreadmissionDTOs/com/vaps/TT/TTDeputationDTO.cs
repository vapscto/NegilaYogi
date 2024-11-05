using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.TT
{
    public class TTDeputationDTO
    {
        public bool returnval { get; set; }
        public string returnduplicatestatus { get; set; }
        //for save 
        public long TTSD_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public DateTime TTSD_Date { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long TTMD_Id { get; set; }
        public int TTMP_Id { get; set; }
        public long TTSD_AbsentStaff { get; set; }
        public long TTSD_DeputedStaff { get; set; }
        public string TTSD_Remarks { get; set; }

        public string deviceid { get; set; }
        public string ASMCL_ClassName { get; set; }
        public bool ASMCL_ActiveFlag { get; set; }
        public bool absflag { get; set; }


        public long ISMS_Id { get; set; }
        //public long MI_Id { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string ISMS_SubjectCode { get; set; }
        public bool ISMS_ActiveFlag { get; set; }

        public Array absentstfcnt { get; set; }
        public Array acayear { get; set; }
        public Array categorylist { get; set; }
        public Array dptdetails { get; set; }
        
        public Array secdrp { get; set; }
        public Array weeklycntlist { get; set; }
        public Array classlist { get; set; }
        public Array stafflist { get; set; }
        public Array periodslst { get; set; }
        public Array gridweeks { get; set; }
        public Array daylist { get; set; }
        public Array absentdpcount { get; set; }
        public Array stfdayperiodlist { get; set; }
        public Array stfweeklyperiodlist { get; set; }
        public Array freeperioddaily { get; set; }
        public Array freeperiodweekly { get; set; }

        public Array Time_Table { get; set; }
        public Array Time_Table_substitute { get; set; }
        public TTDeputationDTO[] staffarray { get; set; }
        public TTDeputationDTO[] subarray { get; set; }

        public TTDeputationDTO[] TT { get; set; }
        public Array dpcount { get; set; }

        public long TTMC_Id { get; set; }
        public long TTFGD_Id { get; set; }
        public long TTFG_Id { get; set; }  
        public long HRME_Id { get; set; }
        public string HRME_EmployeeCode { get; set; }
        public string TTMD_DayName { get; set; }
        public string TTMP_PeriodName { get; set; }
        public string staffName { get; set; }
        public string dayName { get; set; }
        public string periodName { get; set; }
        public string deputedstaffName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string TTMC_CategoryName { get; set; }
        public string FLAG { get; set; }
        public bool smsflag { get; set; }
        public bool mailflag { get; set; }
        public bool notify { get; set; }
        public bool NOT_Flag { get; set; }
    }
}
