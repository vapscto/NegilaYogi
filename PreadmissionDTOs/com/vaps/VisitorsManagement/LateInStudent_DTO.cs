using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.VisitorsManagement
{
    public class LateInStudent_DTO : CommonParamDTO
    {
        public long ALIEOS_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public DateTime ALIEOS_AttendanceDate { get; set; }
        public DateTime ALIEOS_PunchDate { get; set; }
        public string ALIEOS_PunchTime { get; set; }
        public string ALIEOS_Reason { get; set; }
        public string ALIEOS_SystemIP { get; set; }
        //public string ALIEOS_NetworkIP { get; set; }
        public string studentName { get; set; }

        public LateInStudent_DTO[] studetdatalist { get;set;}
        public LateInStudent_DTO[] selectedClasslist { get;set;}
        public LateInStudent_DTO[] selectedSectionlist { get;set;}
        public bool returnval { get; set; }
        public Array academicYear { get; set; }
        public Array classList { get; set; }
        public Array sectionList { get; set; }
        public Array studentlist { get; set; }
        public Array reportlist { get; set; }
        public Array getstudentlist { get; set; }
        public Array editlist { get; set; }

        public string Type { get; set; }
        public string AttendanceFromDate { get; set; }
        public string AttendanceToDate { get; set; }
        public Array month_list { get; set; }
        public int monthid { get; set; }
        public string monthname { get; set; }
        public string all1 { get; set; }
        public string month_id { get; set; }

    }
}
