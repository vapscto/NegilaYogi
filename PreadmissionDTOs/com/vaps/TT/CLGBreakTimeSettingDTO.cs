using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.TT
{
    public class CLGBreakTimeSettingDTO
    {
        public bool returnval { get; set; }
        public string returnduplicatestatus { get; set; }
      
        public int ASMAY_Order { get; set; }
        public string TTMD_DayName { get; set; }
        public long TTMB_Id { get; set; }
        public long TTMBC_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long TTMD_Id { get; set; }
        public long TTMC_Id { get; set; }
        public decimal TTMBC_AfterPeriod { get; set; }
        public string TTMBC_BreakName { get; set; }
        public string TTMBC_BreakStartTime { get; set; }
        public string TTMBC_BreakEndTime { get; set; }
        public long ASMAY_Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Array breaktimelist { get; set; }
        public Array academiclist { get; set; }
        public Array breaktimelistedit { get; set; }
        public Array catelist { get; set; }
        public Array classDrpDwn { get; set; }
        public Array daysDrpDwn { get; set; }
        public long classid { get; set; }
        public long classidscount { get; set; }
        public long classidscountreturn { get; set; }
        public CLGBreakTimeSettingDTO[] ArrayClassList { get; set; }
        public CLGBreakTimeSettingDTO[] ArrayDayList { get; set; }
        public CLGBreakTimeSettingDTO[] ArraybeforeperiodsList { get; set; }
        public CLGBreakTimeSettingDTO[] ArrayafterperiodsList { get; set; }
        public Array categorylist { get; set; }
        public Array yearlist { get; set; }
        public Array courselist { get; set; }
        public Array Daydetailedit { get; set; }
        public long TTMDC_Id { get; set; }
      
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public Array Daylistedit { get; set; }
        public Array branchlist { get; set; }
        public Array daydropdown { get; set; }
        public string AMB_BranchName { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMSE_SEMName { get; set; }
        public string ASMAY_Year { get; set; }
        public long key { get; set; }
        public string TTPeriodnameB { get; set; }
        public string TTPeriodnameA { get; set; }
        public string ASMAYYear { get; set; }
        public string ClassName { get; set; }
        public string DayName { get; set; }
        public string comparevlue { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string TTMC_CategoryName { get; set; }
        public Array classbycategory { get; set; }
        public bool TTMB_ActiveFlag { get; set; }
        public bool TTMBC_ActiveFlag { get; set; }
    }
}
