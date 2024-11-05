using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Exam
{
    public class CollegeBMCPUProgresscardReportDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long ACSS_Id { get; set; }
        public long ACST_Id { get; set;}
        public long EME_Id { get; set; }
        public Array getyear { get; set; }
        public Array getcourse { get; set; }
        public Array getbranch { get; set; }
        public Array getsemester { get; set; }
        public Array getsection { get; set; }
        public Array getsubjectscheme { get; set; }
        public Array getschemetype { get; set; }
        public Array getexam { get; set; }
        public Array instname { get; set; }
        public Array Work_attendence { get; set; }
        public Array Present_attendence { get; set; }
        public Array savelisttot { get; set; }
        public Array subjlist { get; set; }
        public Array grade_details { get; set; }
        public Array subsubject { get; set; }
        public Array studlist { get; set; }
        public Array studmaplist { get; set; }
        public Array gtdetailsview { get; set; }
        public Array edclasslist { get; set; }

        public long AMCST_Id { get; set; }
        public string AMCST_FirstName { get; set; }
        public DateTime AMCST_DOB { get; set; }
        public long ACYST_RollNo { get; set; }
        public string AMCST_AdmNo { get; set; }
        public string AMCST_RegistrationNo { get; set; }
        public long ISMS_Id { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string ISMS_SubjectCode { get; set; }
        public bool ECYSES_AplResultFlg { get; set; }
        public decimal ECYSES_MaxMarks { get; set; }
        public decimal ECYSES_MinMarks { get; set; }
        public int EMGR_Id { get; set; }
        public decimal ECSTMPSSS_MaxMarks { get; set; }
        public decimal ECSTMPS_SemAverage { get; set; }
        public decimal ECSTMPS_SectionAverage { get; set; }
        public decimal ECSTMPS_SemHighest { get; set; }
        public decimal ECSTMPS_SectionHighest { get; set; }
        public decimal ASA_ClassHeld { get; set; }
        public decimal ASA_Class_Attended { get; set; }
        public decimal ECSTMPS_ObtainedMarks { get; set; }
        public string ECSTMPS_ObtainedGrade { get; set; }
        public string ECSTMPS_PassFailFlg { get; set; }
        public string EME_ExamName { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMB_BranchName { get; set; }
        public string AMSE_SEMName { get; set; }
        public string ACMS_SectionName { get; set; }
        public string EMGD_Remarks { get; set; }
        public decimal ECSTMP_TotalObtMarks { get; set; }
        public decimal ECSTMP_Percentage { get; set; }
        public string ECSTMP_TotalGrade { get; set; }
        public int ECSTMP_SemRank { get; set; }
        public int ECSTMP_SectionRank { get; set; }
        public string TotalGrade { get; set; }
        public decimal ECSTMP_TotalMaxMarks { get; set; }
        public int ECYSES_SubjectOrder { get; set; }
        public bool ECYSES_MarksDisplayFlg { get; set; }
        public bool ECYSES_GradeDisplayFlg { get; set; }
        public string ECSTMP_Result { get; set; }
        public string EMSE_SubExamName { get; set; }
        public decimal ECSTMPSSS_ObtainedMarks { get; set; }
        public List<CollegeBMCPUProgresscardReportDTO> savelist { get; set; }
    }
}
