using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Documents
{
   public class NAAC_AC_Criteria_MarksSlab_DTO
    {
        public long NCACCRMRSLB_Id { get; set; }
        public long MI_Id { get; set; }
        public long NAACSL_Id { get; set; }
        public decimal NCACCRMRSLB_FromSlab { get; set; }
        public decimal NCACCRMRSLB_ToSlab { get; set; }
        public decimal NCACCRMRSLB_Marks { get; set; }
        public bool NCACCRMRSLB_ActiveFlg { get; set; }
        public long NCACCRMRSLB_CreatedBy { get; set; }
        public long NCACCRMRSLB_UpdatedBy { get; set; }
        public DateTime NCACCRMRSLB_CreatedDate { get; set; }
        public DateTime NCACCRMRSLB_UpdatedDate { get; set; }
        public long UserId { get; set; }

        public long MT_Id { get; set; }
        public string NAACSL_SLNo { get; set; }
        public string NAACSL_SLNoDescription { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }

        public Array criterialist { get; set; }
        public Array editlist { get; set; }
        public Array griddata { get; set; }

    }
}
