using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Portals.Student
{
    public class StudentHallticketDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMST_Id { get; set; }
        public long EME_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMAY_Rollno { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_AdmNo { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string EHT_HallTicketNo { get; set; }
        public string AMST_Photoname { get; set; }
        public string AMST_FatherName { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string EME_ExamName { get; set; }
        public string ETTS_SessionName { get; set; }
        public string ETTS_StartTime { get; set; }
        public string ETTS_EndTime { get; set; }
        public string htmldata { get; set; }
        public long subjectorder { get; set; }
        public DateTime? EXTTS_Date { get; set; }
        public Array getyearlist { get; set; }
        public Array getcurrentyearlist { get; set; }
        public Array getexamlist { get; set; }
        public Array datareport { get; set; }
        public Array subarray { get; set; }
        public Array studarray { get; set; }
        public Array institute { get; set; }
        public Array configuraion { get; set; }
    }
}
