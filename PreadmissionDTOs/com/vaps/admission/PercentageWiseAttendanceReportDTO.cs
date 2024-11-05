using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class PercentageWiseAttendanceReportDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long userid { get; set; }
        public long roleid { get; set; }
        public string username { get; set; }
        public long HRME_Id { get; set; }
        public Array getyear { get; set; }
        public Array getclass { get; set; }
        public Array getsection { get; set; }
        public Array getreportdetails { get; set; }
        public Array getinstituion { get; set; }
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
        public string allorindi { get; set; }
        public string percentage { get; set; }
        public string flag { get; set; }
        public Temp_studentlist[] Temp_studentlist { get; set; }
        public bool? smschecked { get; set; }
        public bool? emailchecked { get; set; }
        public bool? whatsappchecked { get; set; }
    }

    public class Temp_studentlist
    {
        public long AMST_Id { get; set; }
        public string studentname { get; set; }
        public long AMST_MobileNo { get; set; }
        public string AMST_emailId { get; set; }
        public string classname { get; set; }
        public string sectionname { get; set; }
        public decimal? totalpresentdays { get; set; }
        public decimal? totalworkingdays { get; set; }
        public decimal? percentage { get; set; }
    }
}
