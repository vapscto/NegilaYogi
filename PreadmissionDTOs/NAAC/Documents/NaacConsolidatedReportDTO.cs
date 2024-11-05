using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Documents
{
    public class NaacConsolidatedReportDTO
    {
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public Array getinstitutioncycle { get; set; }
        public long cycleid { get; set; }
        public string cyclename { get; set; }
        public Array getinstitution { get; set; }
        public string NAACSL_InstitutionTypeFlg { get; set; }
        public Array getparentidzero { get; set; }
        public Array getalldata { get; set; }
        public Array getsavealldata { get; set; }
        public decimal percentage { get; set; }

    }
}
