using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class SiblingEmployeeStudentReportDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public Array getyearlist { get; set; }
        public string reporttype { get; set; }
        public Array getreportdetails { get; set; }
        public Array getinstitutiondetails { get; set; }
    }
}
