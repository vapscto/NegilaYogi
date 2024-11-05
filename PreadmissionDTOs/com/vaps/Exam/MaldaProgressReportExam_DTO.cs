using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Exam
{
   public class MaldaProgressReportExam_DTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long HRME_Id { get; set; }
        public int EME_Id { get; set; }
        public decimal? ESTMPS_ObtainedMarks { get; set; }
        public string ESTMPS_ObtainedGrade { get; set; }
        public decimal? ESTMPS_ClassAverage { get; set; }
        public decimal? ESTMPS_SectionAverage { get; set; }      
        public string ESTMPS_PassFailFlg { get; set; }
        public string EME_ExamName { get; set; }
        public string ASMCL_ClassName { get; set; }
        public string ASMC_SectionName { get; set; }
        public long AMST_Id { get; set; }
        public string AMST_FirstName { get; set; }        
        public DateTime? AMST_DOB { get; set; }
        public long AMAY_RollNo { get; set; }
        public string AMST_AdmNo { get; set; }
        public string AMST_RegistrationNo { get; set; }
        public long ISMS_Id { get; set; }
        public string ISMS_SubjectName { get; set; }
        public decimal? ESTMPS_MaxMarks { get; set; }
        public decimal? ESTMPS_SectionHighest { get; set; }
        public string ISMS_SubjectCode { get; set; }
        public bool EYCES_AplResultFlg { get; set; }
        public decimal? EYCES_MaxMarks { get; set; }
        public decimal? EYCES_MinMarks { get; set; }
        public int EMGR_Id { get; set; }
        public decimal classheld { get; set; }
        public decimal classatt { get; set; }
        public string graderemark { get; set; }
        public decimal? ESTMP_TotalObtMarks { get; set; }
        public decimal? ESTMP_Percentage { get; set; }
        public string ESTMP_TotalGrade { get; set; }
        public int? ESTMP_ClassRank { get; set; }
        public int? ESTMP_SectionRank { get; set; }
        public string ESTMP_TotalGradeRemark { get; set; }
        public decimal? ESTMP_TotalMaxMarks { get; set; }
        public int EYCES_SubjectOrder { get; set; }
        public bool EYCES_MarksDisplayFlg { get; set; }
        public bool EYCES_GradeDisplayFlg { get; set; }
        public string ESTMP_Result { get; set; }
        public Array Work_attendence { get; set; }
        public Array Present_attendence { get; set; }
        public Array savelisttot { get; set; }
        public Array grade_details { get; set; }
        public Array promotiondetails { get; set; }
        public decimal? ESTMPS_ClassHighest { get; set; }
        public List<MaldaProgressReportExam_DTO> savelist { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public string EMGD_Name { get; set; }
        public decimal EMGD_From { get; set; }
        public decimal EMGD_To { get; set; }
        public string EMGD_Remarks { get; set; }
        public Array yearlist { get; set; }
        public Array grade_list { get; set; }
        public Array grade_detailslist { get; set; }
        public Array seclist { get; set; }
        public Array classlist { get; set; }
        public Array exmstdlist { get; set; }
        public Array instname { get; set; }
        public Array clstchname { get; set; }
        public Array countStudent { get; set; }
        public Array getstudentdetails { get; set; }
        public Array getsubjectlist { get; set; }
        public Array getexamlist { get; set; }
        public Array getsavedlist { get; set; }
        public Array getattendanceranklist { get; set; }
        public Array remarks { get; set; }
        public Array subjlist { get; set; }
        public Array subsubject { get; set; }
        public Array getgrouplist { get; set; }
        public int EMCA_Id { get; set; }
        public Array getstudentwiseremarks { get; set; }
        public Array getoveralldetails { get; set; }
    }
}
