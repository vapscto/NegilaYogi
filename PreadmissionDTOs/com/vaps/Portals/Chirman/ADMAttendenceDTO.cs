using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Portals.Chirman
{
    public class ADMAttendenceDTO
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
        public long HRME_ID { get; set; }
        public long user_id { get; set; }
        public string AMST_FirstName { get; set; }
    }
}


