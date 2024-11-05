using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class VikasaHallTicketReportDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public long EME_Id { get; set; }
        public Array datareport { get; set; }
        public Array configuraion { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_AdmNo { get; set; }
        public long  AMAY_Rollno { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string EHT_HallTicketNo { get; set; }
        public string ISMS_SubjectName { get; set; }
        public DateTime? EXTTS_Date { get; set; }
        public string EME_ExamName { get; set; }
        public string ETTS_SessionName { get; set; }
        public string AMST_Photoname { get; set; }
        public Array Acdlist { get; set; }
        public Array catlist { get; set; }
        public Array ctlist { get; set; }
        public Array seclist { get; set; }
        public Array examlist { get; set; }
        public Array subarray { get; set; }
        public Array studarray { get; set; }
        public Array institute { get; set; }
        public long subjectorder { get; set; }
        public string AMST_FatherName { get; set; }
        public string htmldata { get; set; }
        public string ETTS_StartTime { get; set; }
        public string ETTS_EndTime { get; set; }
        public Array getstudentlist { get; set; }
        public bool? EHT_PublishFlg { get; set; }
        public VikasaHallTicketReportTempDTO[] studentlist { get; set; }
    }


    public class VikasaHallTicketReportTempDTO
    {
        public long AMST_Id { get; set; }
        public string studentname { get; set; }
    }
}
