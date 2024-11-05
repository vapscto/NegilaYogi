using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Portals.Employee
{
    public class EmployeePortalTimeTableDTO
    {
        public class Input
        {
            public long MI_Id { get; set; }
            public long HRME_Id { get; set; }
            public long ASMAY_Id { get; set; }
          

        }
        public class Output
        {
            public long HRME_Id { get; set; }
            public Array TT_final_generation { get; set; }
            public Array allperiods { get; set; }
            public Array periods { get; set; }
            public Array class_sectons { get; set; }
            
        }
        public class OutputAllPeriods
        {
            public long TTMD_Id { get; set; }
            public string TTMD_DayName { get; set; }
        }
        public class OutputPeriods
        {
            public Int32  TTMP_Id { get; set; }
            public string TTMP_PeriodName { get; set; }
        }
        public class OutputD
        {
            public DateTime? punchdate { get; set; }
            public string punchtime { get; set; }
            public string InOutFlg { get; set; }

        }
        public class OutputClass_section
        {
            public string P_Days { get; set; }
            public string Period { get; set; }
            public string ASMCL_ClassName { get; set; }
            public string ASMC_SectionName { get; set; }
        }
        public class Output_TTGenration
        {
            public string DayName { get; set; }
            public int PeriodCount { get; set; }
        }
        
        public Array EmployeePortalTimeTableD { get; set; }
    }
}
