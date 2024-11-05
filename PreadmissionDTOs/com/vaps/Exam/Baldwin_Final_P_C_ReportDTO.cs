using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class Baldwin_Final_P_C_ReportDTO
    {
        public long MI_Id { get; set; }
        public Array yearlist { get; set; }
        public Array classlist { get; set; }
        public Array sectionlist { get; set; }
        public Array studentlist { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public string AMST_FirstName { get; set; }
        public string AMST_AdmNo { get; set; }
        public long AMAY_RollNo { get; set; }
        public DateTime AMST_DOB { get; set; }
        public Array examlist { get; set; }
        public Array subjectlist { get; set; }
        public Array studentmarks { get; set; }
        public Array classteacher { get; set; }
        public long HRME_Id { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public int EME_Id { get; set; }
        public string EME_ExamName { get; set; }
        public string EME_ExamCode { get; set; }
        public int EME_ExamOrder { get; set; }
        public bool EME_FinalExamFlag { get; set; }
        public int EYCE_Id { get; set; }
        public int EMGR_Id { get; set; }
        public Array examsubjectwise_details { get; set; }
        public Array process_examdetails { get; set; }
        public Array Work_attendence { get; set; }
        public Array Present_attendence { get; set; }
        public bool ExmConfig_PromotionFlag { get; set; }
        public string EMP_MarksPerFlg { get; set; }
        public Array promotion_subectdetails { get; set; }
        public Array prom_subj_groupdetails { get; set; }
        public Array prom_subj_grp_exms { get; set; }
        // public Array grade_details { get; set; }
        public Array promotion_stumarks { get; set; }
        public Array promotion_stumarks_grpwise { get; set; }
        public Array prom_subj_groupdetails_all { get; set; }
        public Array promotion_mainmarks { get; set; }
        public Array prom_cumulativedetails { get; set; }
    }
   
}
