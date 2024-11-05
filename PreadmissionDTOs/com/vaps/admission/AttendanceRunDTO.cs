using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class AttendanceRunDTO
    {
        public long MI_Id { get; set; }
        public Array academic { get; set; }
        public Array viewlist { get; set; }
        public Array griddto { get; set; }
        public long ASMAY_Id { get; set; }
        public DateTime Date { get; set; }
        
        public classlsttwo22[] studentlist { get; set; }
    }

    public class classlsttwo22
    {
        public long ASMCL_Id { get; set; }
        public string ASMS_Id { get; set; }
        public long AMST_Id { get; set; }
    }


}
