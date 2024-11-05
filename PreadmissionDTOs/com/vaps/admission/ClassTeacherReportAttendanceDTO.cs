using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class ClassTeacherReportAttendanceDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public Array getyear { get; set; }
        public Array getyear1 { get; set; }
        public string Flag { get; set; }
        public Array SearchstudentDetails { get; set; }
        public Array category_list { get; set; }
        public bool categoryflag { get; set; }
        public long? AMC_Id { get; set; }
    }
}
