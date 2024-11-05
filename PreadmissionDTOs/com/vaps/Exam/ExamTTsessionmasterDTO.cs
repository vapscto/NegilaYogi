using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class ExamTTsessionmasterDTO
    {
        public long MI_id { get; set; }
        public long ETTS_Id { get; set; }        
        public string ETTS_SessionName { get; set; }
        public string ETTS_StartTime { get; set; }
        public string ETTS_EndTime { get; set; }
        public string ETTS_Abreviation { get; set; }
        public string message { get; set; }
        public bool ETTS_Activeflag { get; set; }
        public bool returnval { get; set; }
        public Array getdetails { get; set; }
        public Array editlist { get; set; }
    }
}
