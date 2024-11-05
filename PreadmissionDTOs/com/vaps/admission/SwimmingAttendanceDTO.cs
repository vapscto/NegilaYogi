using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.admission
{
    public class SwimmingAttendanceDTO
    {
        public long ALSSC_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public DateTime? ASSC_AttendanceDate { get; set; }
        public DateTime? ASSC_PunchDate { get; set; }
        public TimeSpan? ASSC_PunchTime { get; set; }
        public string ASSC_SystemIP { get; set; }
        public string ASSC_NetworkIP { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public string ASSC_EntryForFlg { get; set; }
        public Array getyear { get; set; }
        public Array getclass { get; set; }
        public Array getsection { get; set; }
        public Array getstudent { get; set; }
        public Array getsavedsstudent { get; set; }
        public Array getstandarad { get; set; }
        public Array admissionstandarad { get; set; }
        public bool returnval { get; set; }        
        public long roleId { get; set; }
        public string username { get; set; }
        public string rolename { get; set; }
        public long Emp_Code { get; set; }
        public Tempstudent[] Tempstudent { get; set; }
        public string ASA_Network_IP { get; set; }
        public long userId { get; set; }
        public string flag { get; set; }
    }
    public class Tempstudent
    {
        public long ALSSC_Id { get; set; }      
        public long AMST_Id { get; set; }
        public decimal ALSSC_AttendanceCount { get; set; }
    }
}
