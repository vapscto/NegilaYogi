using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Portals.Chirman
{
    public class Ch_DatewiseAttendanceDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public Array yearlist { get; set; }
        public long asmcL_Id { get; set; }
        public string classname { get; set; }
        public Array fillclass { get; set; }
        public long asmS_Id { get; set; }
        public Array fillsection { get; set; }
        public string sectionname { get; set; }
        public int type { get; set; }
        public Array allstudent { get; set; }
        public string condition { get; set; }
        public decimal value { get; set; }
        public decimal present { get; set; }
        public decimal perc { get; set; }
        public decimal classheld { get; set; }
        public long amstid { get; set; }
        public string studentname { get; set; }
        public Array Fillstudents { get; set; }
        public long monthid { get; set; }
        public string monthname { get; set; }
        public Array fillmonths { get; set; }
        public Array attendencelist { get; set; }
        public Array classarray { get; set; }
        public string Class_Name { get; set; }
        public DateTime? fromdate { get; set; }
        public DateTime? todate { get; set; }
        public string name { get; set; }
        public string admno { get; set; }
        public int rollno { get; set; }

    }
}


