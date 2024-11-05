using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.TT
{
    public class TT_ConsecutiveDTO
    {
        public bool returnval;
        public string returnduplicatestatus;

        public long TTC_Id { get; set; }
        public long MI_Id { get; set; }
        public long TTMC_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long HRME_Id { get; set; }
        public long ISMS_Id { get; set; }
        public decimal TTC_NoOfPeriods { get; set; }
        public decimal TTC_AllotPeriods { get; set; }
        public decimal TTC_RemPeriods { get; set; }
        public decimal TTC_NoOfConPeriods { get; set; }
        public decimal TTC_NoOfConDays { get; set; }
        public int TTC_BefAftApplFlag { get; set; }
        public int TTC_BefAftFalg { get; set; }
        public decimal TTC_BefAftPeriod { get; set; }
        public string TTC_AllotedFlag { get; set; }
        public long ASMAY_Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Array consecutivelst { get; set; }
        public Array consecutivelstedit { get; set; }
        public Array catelist { get; set; }
        public Array academiclist { get; set; }
        public Array classDrpDwn { get; set; }
        public Array sectDrpDwn { get; set; }

        public Array staffDrpDwn { get; set; }
        public Array ttsujectslist { get; set; }
        public Array subjDrpDwn { get; set; }
        public bool TTC_ActiveFlag { get; set; }
        public string ASMAYYear { get; set; }
        public string CategoryName { get; set; }
        public decimal NoOfPeriods { get; set; }
        public string ClassName { get; set; }
        public string SectionName { get; set; }
        public string staffName { get; set; }
        public string SubjectName { get; set; }
        public Array classbycategory { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string TTMC_CategoryName { get; set; }     
        public string staffNamelst { get; set; }
     
     
    }
}
