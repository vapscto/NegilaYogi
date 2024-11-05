using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
    public class SeatallotmentReportDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long Noofyears { get; set; }
        public long UserId { get; set; }
        public Array getyear { get; set; }
        public Array getdetails { get; set; }
        public Array getyearlist { get; set; }

        public Array getinstitutioncycle { get; set; }
        public Array getinstitution { get; set; }
        public string NAACSL_InstitutionTypeFlg { get; set; }
        public long cycleid { get; set; }
        public SeatallotmentReportDTO[] selected_Inst { get; set; }
    }
}
