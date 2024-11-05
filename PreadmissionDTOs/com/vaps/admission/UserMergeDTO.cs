using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class UserMergeDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMST_HRME_Id { get; set; }
        public Array getstudentdetails { get; set; }
        public Array getuserdetails { get; set; }
        public string studentname { get; set; }
        public string admno { get; set; }
        public long AMST_Id { get; set; }
    }
}
