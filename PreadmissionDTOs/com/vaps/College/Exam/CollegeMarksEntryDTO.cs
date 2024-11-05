using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Exam
{
    public class CollegeMarksEntryDTO
    {
        public long ASMAY_Id { get; set; }
        public long ECSTM_Id { get; set; }
        public long MI_Id { get; set; }
        public Array courseslist { get; set; }
        public Array subjectlist { get; set; }
        public Array branchlist { get; set; }
        public Array semisters { get; set; }
        public Array subjectgrplist { get; set; }
        public Array examlist { get; set; }
        public Array sectionlist { get; set; }
        public long AMCO_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACSS_Id { get; set; }
        public long ACST_Id { get; set; }
        public int EME_Id { get; set; }
        public long ISMS_Id { get; set; }
        public int EMGR_Id { get; set; }
        public string EME_ExamName { get; set; }
        public string EME_ExamCode { get; set; }
        public int EME_ExamOrder { get; set; }
        public string SubjectName { get; set; }
        public string AMCST_FirstName { get; set; }
        public string AMCST_MiddleName { get; set; }
        public string AMCST_LastName { get; set; }
        public string amcsT_AdmNo { get; set; }
        public int acysT_RollNo { get; set; }
        public string studentname { get; set; }
        public decimal? TotalMarks { get; set; }
        public decimal? MarksEnterFor { get; set; }
        public decimal? MinMarks { get; set; }
        public string obtainmarks { get; set; }
        public Array studentList { get; set; }
        public Array gradname { get; set; }
        public string subMorGFlag { get; set; }
        public string grade { get; set; }
        public string messagesaveupdate { get; set; }
        public string marksdeleteflag { get; set; }
        public marksDTO[] detailsList { get; set; }
        public string IP4 { get; set; }
        public long Id { get; set; }
        public string username { get; set; }
        public long userId { get; set; }
        public string flag { get; set; }
        public long roleId { get; set; }
        public string rolename { get; set; }
        public string message { get; set; }
        public long Emp_Code { get; set; }
        public Array subjectgroups { get; set; }
        public long EMG_Id { get; set; }
        public Array getyear { get; set; }
        public Array subject_details { get; set; }
        public string ECYSES_MarksGradeEntryFlg { get; set; }
        public bool ECYSES_SubSubjectFlg { get; set; }
        public bool ECYSES_SubExamFlg  { get;set;}
        public Array subject_subsubjects { get; set; }
        public Array subject_subexams { get; set; }
        public Array subsubjectlist { get; set; }
        public Array subexamlist { get; set; }
        public Array grade_details { get; set; }
        public Array subsubject_gradedetails { get; set; }
        public Array subexam_gradedetails { get; set; }
        public Array saved_studentList { get; set; }
        public Array configuration { get; set; }
        public Array saved_ssse_list { get; set; }
        public Array saved_ss_list { get; set; }
        public Array saved_se_list { get; set; }
        public string ECSTM_MarksGradeFlg { get; set; }
        public College_temp_marks_DTO[] main_save_list { get; set; }
        public College_Temp_subs_marks_DTO[] Temp_subs_marks_list { get; set; }
        public bool returnval { get; set; }
        public Array getschemetype { get; set; }
        public Array getsubjectschemetype { get; set; }         
        public long ECYSESSS_Id { get; set; }
        public long EMSS_Id { get; set; }
        public long EMSE_Id { get; set; }
    }
    public class collegemarksDTO
    {
        public long amcsT_Id { get; set; }
        public decimal? totalMarks { get; set; }
        public decimal? minMarks { get; set; }
        public string obtainmarks { get; set; }

    }
    public class College_temp_marks_DTO
    {
        public long AMCST_Id { set; get; }
        public string AMCST_FirstName { get; set; }
        public string AMCST_AdmNo { get; set; }
        public string AMCST_RegistrationNo { get; set; }
        public long ACYST_RollNo { get; set; }
        public long ISMS_Id { set; get; }
        public string ISMS_SubjectName { get; set; }
        public decimal? ECYSES_MaxMarks { get; set; }
        public decimal? ECYSES_MinMarks { get; set; }
        public decimal? ECYSES_MarksEntryMax { get; set; }
        public decimal ECSTM_Marks { get; set; }
        public string ECSTM_Grade { get; set; }
        public string ECSTM_Flg { get; set; }
        public long ECSTM_Id { get; set; }
        public string param { get; set; }
        public string AMST_Sex { get; set; }
    }
    public class College_Temp_subs_marks_DTO
    {
        public int ESTMSS_Id { get; set; }
        //  public long MI_Id { get; set; }
        //  public int ESTM_Id { get; set; }
        public long AMCST_Id { get; set; }
        public int EMSS_Id { get; set; }
        public long ISMS_Id { get; set; }
        public int EMSE_Id { get; set; }
        public decimal? ECSTMSS_Marks { get; set; }
        public string ECSTMSS_MarksGradeFlg { get; set; }
        public string ECSTMSS_Grade { get; set; }     
        public string ECSTMSS_Flg { get; set; }
        public bool ECSTMSS_ActiveFlg { get; set; }
    }
}

