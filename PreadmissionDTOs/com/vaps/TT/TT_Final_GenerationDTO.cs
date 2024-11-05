using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.TT
{
    public class TT_Final_GenerationDTO
    {
        public string staffName { get; set; }

        public bool returnval { get; set; }
        public long TTFG_Id { get; set; }
        public long MI_Id { get; set; }
        public long TTMC_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long TTFG_VersionNo { get; set; }
        public bool TTFG_ActiveFlag { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Array academiclist { get; set; }
        public Array catelist { get; set; }
        public Array totalcountarray { get; set; }
        public string TTMC_CategoryName { get; set; }
        public long CLSTEACHERCON { get; set; }
        public long STAFFSDK { get; set; }
        public long CLSSDK { get; set; }
        public long STAFF_CONSDK { get; set; }
        public long totalpriodscount { get; set; }
        public long totalallotedcount { get; set; }
        public long totalnotallotedcount { get; set; }
        public Array Workloadpdf { get; set; }
        public Array Workloadpdf1 { get; set; }
        public Array countArray { get; set; }
        public Array Time_Table { get; set; }
        public Array Time_Table_new { get; set; }
        public string EmployeeName { get; set; }
        public string ClassName { get; set; }
        public string SubjectName { get; set; }
        public string SectionName { get; set; }
        public long TotalNoOfPeriods { get; set; }
        public long STAFFSDKP { get; set; }
        public long CLSSDKP { get; set; }
        public long STAFF_CONSDKP { get; set; }
        public TT_Final_GenerationDTO[] TT { get; set; }
        public TT_Final_GenerationDTO[] TT1 { get; set; }
        public TT_Final_GenerationDTO[] TempararyArrayList { get; set; }

        public Array datalst { get; set; }
        public Array tempdata { get; set; }
        public Array periodslst { get; set; }
        public Array gridweeks { get; set; }

        public long StaffID { get; set; }
        public long TTFGD_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long TTMD_Id { get; set; }
        public long TTMP_Id { get; set; }
        public string TTMD_DayName { get; set; }
        public string TTMP_PeriodName { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string ISMS_SubjectName { get; set; }
        public int total_workload { get; set; }
        public Array periodslst1 { get; set; }
        public Array gridweeks1 { get; set; }
        public Array Time_Table1 { get; set; }
        public Array Break_list1 { get; set; }
        public Array Break_list_all1 { get; set; }
        public Array classwise_workload { get; set; }
        public long TTFPD_TotWeekPeriods { get; set; }
        public TT_Final_GenerationDTO[] ttmc_idslist { get; set; }
        public string generatetype { get; set; }
        public Array versionlist { get; set; }
        public string Insname { get; set; }
        public string asmayname { get; set; }
        public string version { get; set; }
        public string categoryname { get; set; }

        //praveen added(06/27/2019)
        public int fixingperiodcnt { get; set; }
        public int fixingperiodstaffcnt { get; set; }
        public int bifurcationcnt { get; set; }
    public int consecutivecnt { get; set; }

          public bool FXPRD { get; set; }
        public bool CLSFXPRD { get; set; }
        public bool BFPRD { get; set; }
        public bool CNSPRD { get; set; }
        public bool THREESDC { get; set; }
        public bool THREESDCREP { get; set; }
        public bool TWOSDC { get; set; }
        public bool TWOSDCREP { get; set; }
        public bool ONESDC { get; set; }
        public bool ONESDCREP { get; set; }
        public bool NONSDC { get; set; }
        public bool AVLPRD { get; set; }
    }
}
