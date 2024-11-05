using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.TT
{
    public class TTClassWiseTTDTO
    {

        public bool returnval { get; set; }

        public long ASMCL_Id { get; set; }
        public long MI_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public bool ASMCL_ActiveFlag { get; set; }


        public long ISMS_Id { get; set; }
        //public long MI_Id { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string ISMS_SubjectCode { get; set; }
        public bool ISMS_ActiveFlag { get; set; }
        public decimal TTMB_AfterPeriod { get; set; }
        public string TTMB_BreakName { get; set; }
        public Array categorylist { get; set; }
        public Array sectionlist { get; set; }
        
        public Array classlist { get; set; }
        public Array acayear { get; set; }
        public Array periodslst { get; set; }
        public Array gridweeks { get; set; }
        public Array Break_list { get; set; }
       
        public Array Break_list_all { get; set; }
        public TTClassWiseTTDTO[] Time_Table { get; set; }       
        public TTClassWiseTTDTO[] classarray { get; set; }
        public TTClassWiseTTDTO[] sectionarray { get; set; }

        public TTClassWiseTTDTO[] TT { get; set; }
        public TTClassWiseTTDTO[] TT_Break_list { get; set; }
        public Array TT_Break_list_all { get; set; }

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
        public Array catelist { get; set; }
    }
}
