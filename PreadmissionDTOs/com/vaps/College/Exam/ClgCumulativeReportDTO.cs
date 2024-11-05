using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Exam
{
   public  class ClgCumulativeReportDTO
    {
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long ACSS_Id { get; set; }
        public long AMSE_ID { get; set; }
        public long EME_Id { get; set; }
        public long ACST_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long MI_Id { get; set; }
        public Array exmstdlist { get; set; }
        public Array courseslist { get; set; }
        public Array branchlist { get; set; }
        public Array semisters { get; set; }
        public Array subjectshemalist { get; set; }
        public Array schmetypelist { get; set; }
        public Array sections { get; set; }
        public Array instname { get; set; }
        public string MI_name { get; set; }
        public decimal? ECSTMPS_ObtainedMarks { get; set; }
        public string ECSTMPS_ObtainedGrade { get; set; }
        public string ECSTMPS_PassFailFlg { get; set; }
        public string EME_ExamName { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMB_BranchName { get; set; }
        public string AMSE_SEMName { get; set; }
        public string ACMS_SectionName { get; set; }
        public long AMST_Id { get; set; }
        public string AMST_FirstName { get; set; }
        public DateTime? AMST_DOB { get; set; }
        public long AMAY_RollNo { get; set; }
        public string AMST_AdmNo { get; set; }
        public long ISMS_Id { get; set; }
        public string ISMS_SubjectName { get; set; }
        public decimal? ECSTMPSSS_MaxMarks { get; set; }
        public decimal? ECSTMPS_SemAverage { get; set; }
        public decimal? ECSTMPS_SectionAverage { get; set; }
        public decimal? ECSTMPS_SectionHighest { get; set; }
        public decimal? ESTMPS_SectionHighest { get; set; }
        public string ISMS_SubjectCode { get; set; }
        public bool ECYSES_AplResultFlg { get; set; }
        public decimal? ECYSES_MaxMarks { get; set; }
        public decimal? ECYSES_MinMarks { get; set; }
        public int EMGR_Id { get; set; }
        public decimal classheld { get; set; }
        public decimal classatt { get; set; }
        public string graderemark { get; set; }
        public decimal? ECSTMP_TotalObtMarks { get; set; }
        public decimal? ECSTMP_Percentage { get; set; }
        public string ECSTMP_TotalGrade { get; set; }
        public int? ECSTMP_SemRank { get; set; }
        public int? ECSTMP_SectionRank { get; set; }
        public string ECSTMP_TotalGradeRemark { get; set; }
        public decimal? ECSTMP_TotalMaxMarks { get; set; }
        public int ECYSES_SubjectOrder { get; set; }
        public string ECSTMP_Result { get; set; }
        public List<ClgCumulativeReportDTO> cmreport { get; set; }
        public Array subjlist { get; set; }
        public List<ClgCumulativeReportDTO> savenonsubjlist { get; set; }
        public Array nonsubjlist { get; set; }
        public Array examsubjectwise_details { get; set; }
        public Array configuration { get; set; }
        public List<ClgCumulativeReportDTO> savelist { get; set; }
        public Array yearlist { get; set; }
        public Array GetStudentList { get; set; }
        public Array GetSubjectList { get; set; }
        public Array studentlist { get; set; }
        public Array GetStudentWiseSubjectMakrs { get; set; }
        public Array GetStudentWiseMakrs { get; set; }
        public Array GetProgressCardReportList { get; set; }
        public string studentname { get; set; }
        public string AMCST_AdmNo { get; set; }
        public string AMCST_RegistrationNo { get; set; }
        public long AMCST_Id { get; set; }
        public Studentlist_temp[] Studentlist_temp { get; set; }
        public Array savelisttot { get; set; }
        public Array Work_attendence { get; set; }
        public Array Present_attendence { get; set; }
    }
    public class Studentlist_temp
    {
        public long AMCST_Id { get; set; }
    }
}