using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class DatewiseAttendanceReportDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long roleId { get; set; }
        public long Emp_Code { get; set; }
        public int ASMCL_Order { get; set; }
        public int ASMC_Order { get; set; }
        public string flag { get; set; }
        public string rolename { get; set; }
        public string username { get; set; }
        public string type { get; set; }
        public string message { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public DateTime fromdate { get; set; }
        public Array yearlist { get; set; }
        public Array classlist { get; set; }
        public Array sectionlist { get; set; }
        public Array reportdata { get; set; }
        public Array reportdatacount { get; set; }
        public Array getyear { get; set; }
        public DateTime todate { get; set; }
        public long num { get; set; }

    }
}
