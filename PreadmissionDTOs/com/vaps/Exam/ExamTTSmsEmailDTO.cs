using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class ExamTTSmsEmailDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long EME_Id { get; set; }
        public long AMST_Id { get; set; }
        public int EMG_Id { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_AdmNo { get; set; }
        public long AMST_MobileNo { get; set; }
        public string AMST_emailId { get; set; }
        public long HRME_Id { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public long HRME_MobileNo { get; set; }
        public string HRME_EmailId { get; set; }
        public string HRME_EmployeeCode { get; set; }

        public string ISMS_SubjectName { get; set; }
        public string ETTS_SessionName { get; set; }
        public DateTime? EXTTS_Date { get; set; }
        public string ETTS_StartTime { get; set; }
        public string ETTS_EndTime { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public string EME_ExamName { get; set; }
        public string s { get; set; }
        public string t { get; set; }

        public ExamTTSmsEmailDTO[] check_save_studdto { get; set; }
        public ExamTTSmsEmailDTO[] check_save_teachdto { get; set; }

        public Array Acdlist { get; set; }
        public Array catlist { get; set; }
        public Array ctlist { get; set; }
        public Array seclist { get; set; }
        public Array examlist { get; set; }
        public Array studentlist { get; set; }
        public Array teacherlist { get; set; }
        public Array generateTT { get; set; }

        public bool returnval { get; set; }
        public string examdate { get; set; }
        public string url { get; set; }
        public Array subject_group { get; set; }
        public string dateexam { get; set; }
        public int ASMC_Order { get; set; }
        public int ASMCL_Order { get; set; }
        public bool emailflag { get; set; }
        public bool smsflag { get; set; }
    }
}
