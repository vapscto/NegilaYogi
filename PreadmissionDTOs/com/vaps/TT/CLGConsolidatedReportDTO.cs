using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.TT
{
    public class CLGConsolidatedReportDTO
    {
        public long AMCO_Id { get; set; }
        public long MI_Id { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string TYPE { get; set; }
        public string rpttyp { get; set; }
        public bool ASMCL_ActiveFlag { get; set; }
       

        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ISMS_Id { get; set; }
        //public long MI_Id { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string ISMS_SubjectCode { get; set; }
        public bool ISMS_ActiveFlag { get; set; }

        public Array acayear { get; set; }
        public Array dayslst_fix { get; set; }
        public Array categorylist { get; set; }
        public Array roomlst { get; set; }
        public Array periodtimelist { get; set; }

        public Array secdrp { get; set; }
        public Array TT_Break_list_all { get; set; }
        public Array classlist { get; set; }
        public Array stafflist { get; set; }
        public Array periodslst { get; set; }
        public Array gridweeks { get; set; }

        public CLGConsolidatedReportDTO[] Time_Table { get; set; }
        public CLGConsolidatedReportDTO[] staffarray { get; set; }
        public CLGConsolidatedReportDTO[] subarray { get; set; }

        public CLGConsolidatedReportDTO[] TT { get; set; }
        public CLGConsolidatedReportDTO[] stfidss { get; set; }
        public CLGConsolidatedReportDTO[] dayidss { get; set; }
        public CLGConsolidatedReportDTO[] periodidss { get; set; }
        public long TTMC_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long TTFGD_Id { get; set; }
        public long TTFG_Id { get; set; }
        public long ACMS_Id { get; set; }
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
        public CLGConsolidatedReportDTO[] periodallocation { get; set; }
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

        public string TTMD_DayCode { get; set; }
        public Array dayslst2 { get; set; }
        public CLGConsolidatedReportDTO[] Time_Table2 { get; set; }
        public CLGConsolidatedReportDTO[] TT2 { get; set; }
        public Array dayslst3 { get; set; }
        public CLGConsolidatedReportDTO[] Time_Table3 { get; set; }
        public CLGConsolidatedReportDTO[] TT3 { get; set; }

    }
}
