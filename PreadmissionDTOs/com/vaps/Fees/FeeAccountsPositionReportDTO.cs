using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Fees
{
    public class FeeAccountsPositionReportDTO
    {
        public Array academicYearList { get; set; }
        public Array classList { get; set; }
        public Array sectionList { get; set; }
        public Array feeconfiguration { get; set; }
        public Array customgrpList { get; set; }
        public Array groupList { get; set; }
        public Array termsList { get; set; }
        public FeeAccountsPositionReportDTO[] selectedCGList { get; set; }
        public FeeAccountsPositionReportDTO[] selectedGroup { get; set; }
        public FeeAccountsPositionReportDTO[] selectedTerm { get; set; }
        public long FMGG_Id { get; set; }
        public long FMG_Id { get; set; }

        public long[] FMT_Ids { get; set; }

        public string FMG_GroupName { get; set; }
        public long FMT_Id { get; set; }
        public long MI_Id { get; set; }
        public string type { get; set; }
        public string Status { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string admNo { get; set; }
        public string studentName { get; set; }
        public string charges { get; set; }
        public string concession { get; set; }
        public string rebate { get; set; }
        public string waiveOff { get; set; }
        public string fine { get; set; }
        public long collection { get; set; }
        public string debitBalance { get; set; }
        public string lastYearDue { get; set; }

        public string PFY_EndDate_DebitBalance { get; set; }
        public long CFY_PaidAmount { get; set; }
        public string CFY_BalanceAmount { get; set; }
        public string ExcessAmount { get; set; }

        public Array feeaccountsPositionReport { get; set; }
        public int count { get; set; }
        public string FeeName { get; set; }
        public string className { get; set; }
        public long User_Id { get; set; }
        public string RouteName { get; set; }

        public DateTime? asondate { get; set; }

        public Array financialyear { get; set; }

        public string yeartype { get; set; }
    }
}
