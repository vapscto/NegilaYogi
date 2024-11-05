using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class MasterClassHeldDTO
    {
        public long ASCH_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long IVRM_Month_Id { get; set; }
        public decimal ASCH_ClassHeld { get; set; }
        public bool ASCH_Active_Flag { get; set; }
        public Array allyear { get; set; }
        public Array currentYear { get; set; }
        public Array classDrpDwn { get; set; }
        public Array monthList { get; set; }
        public Array sectionDrpDwn { get; set; }
        public MonthDTO[] selectedmonthList { get; set; }
        public MasterSectionDTO[] selectedSectionList { get; set; }
        public Array message { get; set; }
        public int msgcount { get; set; }
        public bool returnVal { get; set; }
        public Array NoOfClassHeldCount { get; set; }
       
    }
}
