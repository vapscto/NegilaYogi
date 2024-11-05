using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Library
{
   public class SCHTransactionDetailsReportDTO : CommonParamDTO
    {
        public long LMAL_Id { get; set; }
        public long LMB_Id { get; set; }
        public long MI_Id { get; set; }
        public long IVRMUL_Id { get; set; }
        public string Type { get; set; }
        public string Type2 { get; set; }
        public long LMD_Id { get; set; }
        public string LMB_BookType { get; set; }
        public string LMB_PurOrDonated { get; set; }
        public Array deptlist { get; set; }
        public Array griddata { get; set; }
        public Array msterliblist1 { get; set; }

        public DateTime Fromdate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime retFromdate { get; set; }
        public DateTime retToDate { get; set; }
        public bool statuscount { get; set; }
        public bool statuscount1 { get; set; }

    }
}
