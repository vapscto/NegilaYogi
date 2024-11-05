using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.TT
{
    public class TTConsolidatedDTO
    {
   
        public long MI_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string TYPE { get; set; }
        public bool ASMCL_ActiveFlag { get; set; }

        public TTclassArrayDTO[] TTclassArray { get; set; }
        public TTsectionArrayDTO[] TTsectionArray { get; set; }

        public class TTclassArrayDTO
        {
            public long ASMCL_Id { get; set; }
        }
        public class TTsectionArrayDTO
        {
            public long ASMS_Id { get; set; }
        }


        public albumNameArray1DTO[] albumNameArray1 { get; set; }
        /// public bool ASMCL_Id { get; set; }

        public class albumNameArray1DTO
        {
            public long ASMCL_Id { get; set; }
        }
        public albumNameArray2DTO[] albumNameArray2 { get; set; }
        public class albumNameArray2DTO
        {
            public long ASMS_Id { get; set; }
        }

      


        public decimal TTMB_AfterPeriod { get; set; }
        public string TTMB_BreakName { get; set; }
        public long ISMS_Id { get; set; }
        //public long MI_Id { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string ISMS_SubjectCode { get; set; }
        public bool ISMS_ActiveFlag { get; set; }
        public string rpttyp { get; set; }
        public Array acayear { get; set; }
        public Array dayslst_fix { get; set; }
        public Array class_list { get; set; }
        public Array categorylist { get; set; }
        
              public Array Section_list { get; set; }


        public Array catelist { get; set; }
        public Array clalist { get; set; }
        public Array sectionlist { get; set; }
        public Array sectionlists { get; set; }
        public Array roomlst { get; set; }
        public Array TT_Break_list_all { get; set; }
        public Array secdrp { get; set; }
        public Array classlist { get; set; }
        public Array classlists { get; set; }
        public Array stafflist { get; set; }
        public Array periodslst { get; set; }
        public Array gridweeks { get; set; }
        public Array cateoglst { get; set; }
        
        public Array classlst { get; set; }

        public bool returnval { get; set; }
        public TTConsolidatedDTO[] Time_Table { get; set; }
        public TTConsolidatedDTO[] staffarray { get; set; }
        public TTConsolidatedDTO[] subarray { get; set; }

        public TTConsolidatedDTO[] TT { get; set; }
        public TTConsolidatedDTO[] catedss { get; set; }
        public TTConsolidatedDTO[] sectiondss { get; set; }
        public TTConsolidatedDTO[] classdss { get; set; }

        public TTConsolidatedDTO[] clsidss { get; set; }

        public TTConsolidatedDTO[] secidss { get; set; }


        public TTConsolidatedDTO[] stfidss { get; set; }
        public TTConsolidatedDTO[] dayidss { get; set; }
        public TTConsolidatedDTO[] periodidss { get; set; }
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
        public Array gridstaff { get; set; }
        public Array gridcls { get; set; }
        public Array finaltable { get; set; }
        public long TTMSAB_Id { get; set; }
        public string TTMSAB_Abbreviation { get; set; }
        public bool TTMSAB_ActiveFlag { get; set; }
        public Array stafflst { get; set; }
        public Array dayslst { get; set; }
        public Array emplst { get; set; }
        
               public Array categorylst { get; set; }
        public TTConsolidatedDTO[] periodallocation { get; set; }
        public string empName { get; set; }

        public long TTFPD_Id { get; set; }
        public int TTFPD_TotWeekPeriods { get; set; }
        public bool TTFPD_ActiveFlag { get; set; }

        public long TTFPDD_Id { get; set; }
        public int TTFPD_TotalPeriods { get; set; }
        public int TTFPD_AllotedPeriods { get; set; }
        public int TTFPD_AvailablePeriods { get; set; }
        public bool TTFPDD_ActiveFlag { get; set; }
        public string rpttypairods { get; set; } 

        public Array subjectlist { get; set; }
        public Array class_sectons { get; set; }
        public Array classsslist { get; set; }
         public Array sidss { get; set; }
    
        public string TTMD_DayCode { get; set; }
        public Array dayslst2 { get; set; }
        public TTConsolidatedDTO[] Time_Table2 { get; set; }
        public TTConsolidatedDTO[] TT2 { get; set; }
        public Array dayslst3 { get; set; }
        public TTConsolidatedDTO[] Time_Table3 { get; set; }
        public TTConsolidatedDTO[] TT3 { get; set; }

    }
}
