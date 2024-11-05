using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.admission
{
   public class AdmissionReportDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long UserId { get; set; }
        public string MI_Name { get; set; }
        public Array institutlistnew { get; set; }
        public Array get_Report { get; set; }
        public string Fromdatee { get; set; }
        public string ToDate { get; set; }
        public institute[] institutlist { get; set; }


    }
    public class institute    {        public long mI_Id { get; set; }    }
}
