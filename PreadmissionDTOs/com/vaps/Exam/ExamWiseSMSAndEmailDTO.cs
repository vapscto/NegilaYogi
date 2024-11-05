using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class ExamWiseSMSAndEmailDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long EME_Id { get; set; }
        public long userid { get; set; }
        public long AMST_Id { get; set; }
        public long? MOBILENO { get; set; }
        public string EMAILID { get; set; }
        public string message { get; set; }
        public Array allacademicyear { get; set; }
        public Array allclasslist { get; set; }
        public Array allsectionlist { get; set; }
        public Array allexamlist { get; set; }
        public Array studentdetails { get; set; }
        public ExamWiseSMSAndEmailDTO[] finalstudentlist { get; set; }
        public bool? sms { get; set; }
        public bool? email { get; set; }
        public string typeformat { get; set; }
        public string MARKSDETAILS { get; set; }
    }
}
