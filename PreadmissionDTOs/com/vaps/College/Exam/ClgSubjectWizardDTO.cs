using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Exam
{
    public class ClgSubjectWizardDTO
    {
        public int? EMSS_Id { get; set; }
        public bool ECYSESSS_ActiveFlg { get; set; }
        public int? ECYSESSS_SubSubjectOrder { get; set; }
        public decimal? ECYSESSS_ExemptedPer { get; set; }
        public bool? ECYSESSS_ExemptedFlg { get; set; }
        public int? EMSE_Id { get; set; }
        public long MI_Id { get; set; }
        public Array courseslist { get; set; }
        public Array subjectshemalist { get; set; }
        public Array subjectgrplist { get; set; }
        public Array branchlist { get; set; }
        public Array schmetypelist { get; set; }
        public Array semisters { get; set; }
        public Array gradelist { get; set; }
        public Array examlist { get; set; }
        public Array subexamlist { get; set; }
        public Array subjectlist { get; set; }
        public Array subsubjectlist { get; set; }
        public long ECYSE_Id { get; set; }
        public long ECYS_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long ACSS_Id { get; set; }
        public long ACST_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long EME_Id { get; set; }
        public int? EMGR_Id { get; set; }
        public DateTime? ECYSE_AttendanceFromDate { get; set; }
        public DateTime? ECYSE_AttendanceToDate { get; set; }
        public bool ECYSE_SubExamFlg { get; set; }
        public bool ECYSE_SubSubjectFlg { get; set; }
        public bool ECYSE_ActiveFlg { get; set; }
        public long ISMS_Id { get; set; }
        public Exm_Master_ExamDTO[] exams_list { get; set; }
        public Exm_Yrly_Sch_Exams_SubwiseDTO[] exm_subjects_list { get; set; }
        public tempDTO_subexams[] exm_subject_subexams_list { get; set; }
        public bool returnval { get; set; }
        public string returnduplicatestatus { get; set; }
        public Array Scheme_exams { get; set; }
        public string EME_ExamName { get; set; }
        public string EME_ExamCode { get; set; }
        public string EMGR_GradeName { get; set; }
        public string EMSE_SubExamName { get; set; }
        public string EMSE_SubExamCode { get; set; }
        public string EMSS_SubSubjectName { get; set; }
        public string EMSS_SubSubjectCode { get; set; }
        public Array view_exam_subjects { get; set; }
        public Array view_exam_subjects_subexams { get; set; }
        public Array view_exam_subjects_subsubjects { get; set; }
        public long ECYSES_Id { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string ISMS_SubjectCode { get; set; }
        public decimal ECYSES_MarksEntryMax { get; set; }
        public decimal ECYSES_MaxMarks { get; set; }
        public decimal ECYSES_MinMarks { get; set; }
        public bool ECYSES_SubExamFlg { get; set; }
        public bool ECYSES_SubSubjectFlg { get; set; }
        public string ECYSES_MarksGradeEntryFlg { get; set; }
        public bool ECYSES_MarksDisplayFlg { get; set; }
        public bool ECYSES_GradeDisplayFlg { get; set; }
        public bool ECYSES_AplResultFlg { get; set; }
        public int? ECYSES_SubjectOrder { get; set; }
        public bool ECYSES_ActiveFlg { get; set; }
        public bool already_cnt { get; set; }
        public Array edit_cat_exm { get; set; }
        public Array edit_cat_exm_subs { get; set; }
        public int EMG_ID { get; set; }
        public tempDTO_subsubjects[] exm_subject_subsubjects_list { get; set; }
        public tempDTO_subsubject_subexams[] exm_subject_subsubjects_subexam { get; set; }
        public Array edit_cat_exm_subs_sub_exms { get; set; }
        public Array edit_cat_exm_subs_sub_subjs { get; set; }
        public long ECYSESSS_Id { get; set; }
        public long ESYSES_Id { get; set; }
        public decimal? ECYSESSS_MaxMarks { get; set; }
        public decimal? ECYSESSS_MinMarks { get; set; }
        public string subjectgrpname { get; set; }
        public Array subsubjectsubexamlist { get; set; }
        public int EMSS_Order { get; set; }
        public int EMSE_SubExamOrder { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMB_BranchName { get; set; }
        public string AMSE_SEMName { get; set; }
        public string schemetype { get; set; }
        public string subjectscheme { get; set; }
        public string ECYSESSS_ProgressCardFlag { get; set; }
        public string ECYSESSS_SubjectDisplayName { get; set; }
        public string ECYSESSS_SubjectDisplayCode { get; set; }
        public Array subjectgroups { get; set; }
        public Set_SubSubject_Order_DTO[] Set_SubSubject_Order_DTO { get; set; }
    }
    public class tempDTO_subsubjects
    {
        public long ISMS_Id { get; set; }
        public Exm_Yrly_Sch_Exams_Subwise_SubSubjectsDTO[] sub_subjs_list { get; set; }
    }
    public class Exm_Yrly_Sch_Exams_Subwise_SubSubjectsDTO : CommonParamDTO
    {
        public long ECYSESSS_Id { get; set; }
        public long ECYSES_Id { get; set; }
        public long? EMSS_Id { get; set; }
        public long? EMSE_Id { get; set; }
        public int? EMGR_Id { get; set; }
        public decimal? ECYSESSS_MaxMarks { get; set; }
        public decimal? ECYSESSS_MinMarks { get; set; }
        public bool? ECYSESSS_ExemptedFlg { get; set; }
        public decimal? ECYSESSS_ExemptedPer { get; set; }
        public int? ECYSESSS_SubSubjectOrder { get; set; }
        public bool ECYSESSS_ActiveFlg { get; set; }
        public string ECYSESSS_ProgressCardFlag { get; set; }
        public string ECYSESSS_SubjectDisplayName { get; set; }
        public string ECYSESSS_SubjectDisplayCode { get; set; }
    }
    public class Exm_Master_ExamDTO : CommonParamDTO
    {
        public int EME_Id { get; set; }
        public long MI_Id { get; set; }
        public string EME_ExamName { get; set; }
        public string EME_ExamCode { get; set; }
        public int EME_ExamOrder { get; set; }
        public bool EME_FinalExamFlag { get; set; }
        public bool EME_ActiveFlag { get; set; }
    }
    public class Exm_Yrly_Sch_Exams_SubwiseDTO : CommonParamDTO
    {
        public long ECYSES_Id { get; set; }
        public long ECYSE_Id { get; set; }
        public long ISMS_Id { get; set; }
        public int EMGR_Id { get; set; }
        public decimal ECYSES_MarksEntryMax { get; set; }
        public decimal ECYSES_MaxMarks { get; set; }
        public decimal ECYSES_MinMarks { get; set; }
        public bool ECYSES_SubExamFlg { get; set; }
        public bool ECYSES_SubSubjectFlg { get; set; }
        public string ECYSES_MarksGradeEntryFlg { get; set; }
        public bool ECYSES_MarksDisplayFlg { get; set; }
        public bool ECYSES_GradeDisplayFlg { get; set; }
        public bool ECYSES_AplResultFlg { get; set; }
        public int? ECYSES_SubjectOrder { get; set; }
        public bool ECYSES_ActiveFlg { get; set; }
    }
    public class tempDTO_subexams
    {
        public long ISMS_Id { get; set; }
        public Exm_Yrly_Sch_Exams_Subwise_SubExamsDTO[] sub_exam_list { get; set; }
    }

    public class tempDTO_subsubject_subexams
    {
        public long ISMS_Id { get; set; }
        public Exm_Yrly_Sch_Exams_Subwise_SubExamsDTO[] sub_subject_sub_exam_list { get; set; }
    }
    public class Exm_Yrly_Sch_Exams_Subwise_SubExamsDTO : CommonParamDTO
    {
        public long ECYSESSS_Id { get; set; }
        public long ECYSES_Id { get; set; }
        public int? EMSS_Id { get; set; }
        public int? EMSE_Id { get; set; }
        public int? EMGR_Id { get; set; }
        public decimal? ECYSESSS_MaxMarks { get; set; }
        public decimal? ECYSESSS_MinMarks { get; set; }
        public bool? ECYSESSS_ExemptedFlg { get; set; }
        public decimal? ECYSESSS_ExemptedPer { get; set; }
        public int? ECYSESSS_SubSubjectOrder { get; set; }
        public bool ECYSESSS_ActiveFlg { get; set; }
        public string ECYSESSS_ProgressCardFlag { get; set; }
        public string ECYSESSS_SubjectDisplayName { get; set; }
        public string ECYSESSS_SubjectDisplayCode { get; set; }
    }

    public class Set_SubSubject_Order_DTO
    {
        public long ECYSESSS_Id { get; set; }
        public long ECYSES_Id { get; set; }
        public int? ECYSESSS_SubSubjectOrder { get; set; }
    }
}