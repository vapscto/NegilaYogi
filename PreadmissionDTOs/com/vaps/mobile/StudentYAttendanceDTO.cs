using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.mobile
{
    public class StudentYAttendanceDTO
    {
        public class input
        {
            public long AMST_Id { get; set; }
            public long MI_Id { get; set; }
            public long ASMAY_Id { get; set; }
          //  public int monthid { get; set; }
        }
        //public long ClassHeld { get; set; }
        //public long Class_Attended { get; set; }
        //public long Percentage { get; set; }
        //public Array Presentdays { get; set; }
        //public Array Absentdays { get; set; }
        //public Array FHPdays { get; set; }
        //public Array SHPdays { get; set; }
        public Array YearlyArray { get; set; }


    }
}
