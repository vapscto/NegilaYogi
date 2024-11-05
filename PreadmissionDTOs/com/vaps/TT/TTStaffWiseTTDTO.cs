using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.TT
{
    public class TTStaffWiseTTDTO
    {
        public long ASMCL_Id { get; set; }
        public long MI_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public bool ASMCL_ActiveFlag { get; set; }


        public long ISMS_Id { get; set; }
        //public long MI_Id { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string ISMS_SubjectCode { get; set; }
        public bool ISMS_ActiveFlag { get; set; }

        public Array acayear { get; set; }
        public Array categorylist { get; set; }
        
        public Array secdrp { get; set; }
        public Array classlist { get; set; }
        public Array stafflist { get; set; }
        public Array periodslst { get; set; }
        public Array gridweeks { get; set; }
        
        public TTStaffWiseTTDTO[] Time_Table { get; set; }
        public TTStaffWiseTTDTO[] staffarray { get; set; }
        public TTStaffWiseTTDTO[] subarray { get; set; }

        public TTStaffWiseTTDTO[] TT { get; set; }

        public long TTMC_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long TTFGD_Id { get; set; }
        public long TTFG_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long HRME_Id { get; set; }
        public long TTMD_Id { get; set; }
        public int TTMP_Id { get; set; }
        public string TTMD_DayName { get; set; }
        public string TTMP_PeriodName { get; set; }
        public string staffName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string TTMC_CategoryName { get; set; }
        public Array staffnameswithclas { get; set; }
        public Array nameswithclas { get; set; }
        public string Names { get; set; }
    }
}
