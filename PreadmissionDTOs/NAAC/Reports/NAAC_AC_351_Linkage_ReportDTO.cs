using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Reports
{
   public class NAAC_AC_351_Linkage_ReportDTO
    {
        public long NCAC351LIN_Id { get; set; }
        public long NCAC351LIN_CommYear { get; set; }
        public long MI_Id { get; set; }
        public string NCAC351LIN_FileName { get; set; }
        public string NCAC351LIN_FilePath { get; set; }
        public Array alldata4 { get; set; }
        public Array alldata42 { get; set; }
        public Array alldata5 { get; set; }
        public Array alldata434 { get; set; }
        public Array alldata6 { get; set; }
        public Array alldata62 { get; set; }
        public Array alldata7 { get; set; }
        public Array alldata72 { get; set; }
        public Array alldata8 { get; set; }
        public Array yearlist { get; set; }
        public Array alldata82 { get; set; }
        public CurricularAspects_DTO[] selectedYear { get; set; }
        public Array getinstitutioncycle { get; set; }
        public Array getinstitution { get; set; }
        public long UserId { get; set; }
        public NAAC_AC_351_Linkage_ReportDTO[] selected_Inst { get; set; }
        public long cycleid { get; set; }
        public Array reportlist { get; set; }
        public Array reportlist2 { get; set; }

    }
}
