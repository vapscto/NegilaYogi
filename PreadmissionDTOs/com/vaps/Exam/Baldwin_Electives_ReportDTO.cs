using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class Baldwin_Electives_ReportDTO
    {
        public long MI_Id { get; set; }
        public Array yearlist { get; set; }
        public Array categorylist { get; set; }
        public long ASMAY_Id { get; set; }
        public string Type { get; set; }
        public int EMCA_Id { get; set; }
        public Array grouplist { get; set; }
        public Array studentlist { get; set; }
        public Array studentsubj_list { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_AdmNo { get; set; }
        public long AMAY_RollNo { get; set; }
        public DateTime? AMST_DOB { get; set; }
        // public string AMST_Photoname { get; set; }
        public long AMST_MobileNo { get; set; }
        public string AMST_emailId { get; set; }       
        public string ASMCL_ClassName { get; set; }
        public int ASMCL_Order { get; set; }
        public string ASMC_SectionName { get; set; }
        public int ASMC_Order { get; set; }
        public int EMG_Id { get; set; }
        public string EMG_GroupName { get; set; }
        public long ISMS_Id { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string EME_ExamName { get; set; }
        public Array subjectlist { get; set; }
        public Array classlist { get; set; }
        public Array sectionlist { get; set; }
        public Array instituelist { get; set; }
        public bool? Deactive_Flag { get; set; }
        public bool? Left_Flag { get; set; }
    }
}
