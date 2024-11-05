using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Hostel
{
    public class CLGStudentReportDTO
    {
        public long MI_Id { get; set; }
        public string issuertype1 { get; set; }
        public string frmdate { get; set; }
        public string todate { get; set; }
        public string ctype { get; set; }
        public Array griddata { get; set; }
        public Array yearlist { get; set; }
        public Array hostellist { get; set; }
        public string type { get; set; }
        public long ASMAY_Id { get; set; }
        public long HLMH_Id { get; set; }
    }
}
