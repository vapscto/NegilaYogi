using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Documents
{
   public class NaacDocumentUploadReport_DTO
    {
        public long MI_Id { get; set; }
        public long NAACSL_Id { get; set; }
        public decimal? NAACSL_Percentage { get; set; }
        public string NAACSL_SLNoDescription { get; set; }
        public long NAACSL_ParentId { get; set; }
        public int NAACSL_SLNoOrder { get; set; }
        public string NAACSL_SLNote { get; set; }
        public bool? NAACSL_TextBoxFlg { get; set; }
        public bool? NAACSL_UploadReq { get; set; }
        public string NAACMSL_Status { get; set; }
        public decimal? NAACMSL_CGPA { get; set; }
        public string NAACSL_SLNo { get; set; }
        public Array reportlist { get; set; }
        public Array getparentidzero { get; set; }
        public Array getalldata { get; set; }
        public Array get_Report { get; set; }
        public Array getsavealldata { get; set; }
        public decimal percentage { get; set; }
        public long UserId { get; set; }
        public Array getinstitutioncycle { get; set; }
        public Array getinstitution { get; set; }
        public Array getparentidone { get; set; }
        public long cycleid { get; set; }
    }
}
