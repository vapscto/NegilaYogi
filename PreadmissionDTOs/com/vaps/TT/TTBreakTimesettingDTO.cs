using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.TT
{
    public class TTBreakTimesettingDTO
    {
        public bool returnval { get; set; }
        public string returnduplicatestatus { get; set; }

        public long TTMB_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long TTMD_Id { get; set; }
        public long TTMC_Id { get; set; }
        public decimal TTMB_AfterPeriod { get; set; }
        public string TTMB_BreakName { get; set; }
        public string TTMB_BreakStartTime { get; set; }
        public string TTMB_BreakEndTime { get; set; }
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
        public TTBreakTimesettingDTO[] ArrayClassList { get; set; }
        public TTBreakTimesettingDTO[] ArrayDayList { get; set; }
        public TTBreakTimesettingDTO[] ArraybeforeperiodsList { get; set; }
        public TTBreakTimesettingDTO[] ArrayafterperiodsList { get; set; }
     
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
    }
}
