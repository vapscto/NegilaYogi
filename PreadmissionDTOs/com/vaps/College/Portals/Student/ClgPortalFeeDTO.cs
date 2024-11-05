using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Portals
{
    public class ClgPortalFeeDTO
    {
        public long MI_Id { get; set; }
        public long AMCST_Id { get; set; }
        public int ASMAY_Order { get; set; }
        public Array yearlist { get; set; }
        public Array currentyear { get; set; }
        public long ASMAY_Id { get; set; }
        public string ASMAY_Year { get; set; }
        public string type { get; set; }
        public Array getfeedetails { get; set; }


        public long FYP_Id { get; set; }
        public string FYP_ReceiptNo { get; set; }
        public Array recnolist { get;set; }
        public long User_Id { get; set; }

        public Array feeconfiglist { get; set; }

    }
}
