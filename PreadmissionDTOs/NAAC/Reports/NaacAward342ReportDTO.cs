using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Reports
{
    public class NaacAward342ReportDTO
    {
        public long MI_Id { get; set; }
        public Array allacademicyear { get; set; }
        public Array alldata1 { get; set; }
        public Array alldata12 { get; set; }
        public string ASMAY_Year { get; set; }
        public long ASMAY_Id { get; set; }
        public long NCACAW342_Id { get; set; }
        public string NCACAW342_ActivityName { get; set; }
        public string NCACAW342_AwardName { get; set; }
        public string NCACAW342_AwardingBody { get; set; }
        public long NCACAW342_AwardYear { get; set; }
        public bool NCACAW342_ActiveFlg { get; set; }

        public NaacAward342ReportDTO[] selectedYear {get;set;}
        public Array getinstitutioncycle { get; set; }
        public Array getinstitution { get; set; }
        public long UserId { get; set; }
        public long cycleid { get; set; }
        public Array yearlist { get; set; }
        public Array reportlist { get; set; }
        public Array reportlist2 { get; set; }
        public NaacAward342ReportDTO[] selected_Inst { get; set; }

    }
}
