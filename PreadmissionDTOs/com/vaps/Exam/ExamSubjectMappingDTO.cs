using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class ExamSubjectMappingDTO : CommonParamDTO
    {
        public bool already_cnt { get; set; }

        //cat-exm mapping
        public int EYCE_Id { get; set; }
        public int EYC_Id { get; set; }
        public int EME_Id { get; set; }
        public int EMGR_Id { get; set; }
        public DateTime? EYCE_AttendanceFromDate { get; set; }
        public DateTime? EYCE_AttendanceToDate { get; set; }
        public bool EYCE_SubExamFlg { get; set; }
        public bool EYCE_SubSubjectFlg { get; set; }
        public bool EYCE_ActiveFlg { get; set; }

        //cat-exm-subjs
        public int EYCES_Id { get; set; }
        public long ISMS_Id { get; set; }
        public decimal? EYCES_MarksEntryMax { get; set; }
        public decimal? EYCES_MaxMarks { get; set; }
        public decimal? EYCES_MinMarks { get; set; }
        public bool EYCES_SubExamFlg { get; set; }

        //cat-exm-subjs-subsubjs
        public int EYCESSS_Id { get; set; }
        public int EMSS_Id { get; set; }
        public decimal? EYCESSS_MaxMarks { get; set; }
        public decimal? EYCESSS_MinMarks { get; set; }
        public decimal? EYCESSS_MarksEntryMax { get; set; }
        public bool EYCESSS_ExemptedFlg { get; set; }
        public decimal? EYCESSS_ExemptedPer { get; set; }
        public bool EYCESSS_ActiveFlg { get; set; }

        //cat-exm-subjs-subexms
        public int EYCESSE_Id { get; set; }
        public int EMSE_Id { get; set; }
        public decimal? EYCESSE_MaxMarks { get; set; }
        public decimal? EYCESSE_MinMarks { get; set; }
        public bool EYCESSE_ExemptedFlg { get; set; }
        public decimal? EYCESSE_ExemptedPer { get; set; }
        public bool EYCESSE_ActiveFlg { get; set; }
        public bool EYCES_SubSubjectFlg { get; set; }
        public string EYCES_MarksGradeEntryFlg { get; set; }
        public bool EYCES_MarksDisplayFlg { get; set; }
        public bool EYCES_GradeDisplayFlg { get; set; }
        public bool EYCES_AplResultFlg { get; set; }
        public bool EYCES_ActiveFlg { get; set; }
        public int EYCES_SubjectOrder { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string ISMS_SubjectCode { get; set; }
        public decimal? ISMS_Max_Marks { get; set; }
        public decimal? ISMS_Min_Marks { get; set; }
        public long ISMS_OrderFlag { get; set; }
        public int EYCESSE_SubExamOrder { get; set; }
        public int EYCESSS_SubSubjectOrder { get; set; }
        public int EMCA_Id { get; set; }
        public long MI_Id { get; set; }
        public long UserId { get; set; }
        public long ASMAY_Id { get; set; }
        public bool? EYCE_BestOfApplicableFlg { get; set; }
        public long? EYCE_BestOf { get; set; }
        public string ASMAY_Year { get; set; }
        public string EMCA_CategoryName { get; set; }
        public string EME_ExamName { get; set; }
        public string EME_ExamCode { get; set; }
        public string EMGR_GradeName { get; set; }
        public string EMSE_SubExamName { get; set; }
        public string EMSE_SubExamCode { get; set; }
        public string EMSS_SubSubjectName { get; set; }
        public string EMSS_SubSubjectCode { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public Array yearlist { get; set; }
        public Array categorylist { get; set; }
        public Array gradelist { get; set; }
        public Array examlist { get; set; }
        public Array subexamlist { get; set; }
        public Array subjectlist { get; set; }
        public Array subsubjectlist { get; set; }
        public Array subsubjectsubexamlist { get; set; }
        public int EMSS_Order { get; set; }
        public int EMSE_SubExamOrder { get; set; }
        public Exm_Master_ExamDTO[] exams_list { get; set; }
        public Exm_Yrly_Cat_Exams_SubwiseDTO[] exm_subjects_list { get; set; }
        public tempDTO_subexams[] exm_subject_subexams_list { get; set; }
        public tempDTO_subsubjects[] exm_subject_subsubjects_list { get; set; }
        public tempDTO_subsubjects_subexam[] exm_subject_subsubjects_subexam { get; set; }
        public Array category_exams { get; set; }
        public Array view_exam_subjects { get; set; }
        public Array view_exam_subjects_subexams { get; set; }
        public Array view_exam_subjects_subsubjects { get; set; }
        public Array edit_cat_exm { get; set; }
        public Array edit_cat_exm_subs { get; set; }
        public Array edit_cat_exm_subs_sub_exms { get; set; }
        public Array edit_cat_exm_subs_sub_subjs { get; set; }
        public Array edit_cat_exm_subs_grade_list { get; set; }
        public int ASMAY_Order { get; set; }

        //master exam remove later
        public int EME_ExamOrder { get; set; }
        public bool EME_FinalExamFlag { get; set; }
        public bool EYCESSS_MarksFlg { get; set; }
        public bool EYCESSS_GradesFlg { get; set; }
        public bool? EYCESSS_AplResultFlg { get; set; }
        public bool EME_ActiveFlag { get; set; }
        public Temp_Subject_Order[] Temp_Subject_Order { get; set; }
        public string EYCEAttendanceFromDate { get; set; }
        public string EYCEAttendanceToDate { get; set; }
        public Array Get_Master_PT { get; set; }
        public Array view_exam_subjects_grade_list { get; set; }
        public int EMPATY_Id { get; set; }
        public string EMPATY_PaperTypeName { get; set; }
        public bool? EYCESPT_ActiveFlg { get; set; }
        public int EYCESPT_Id { get; set; }
        public long count { get; set; }
        public bool? edit_exam_flag { get; set; }
    }

    public class tempDTO_subexams
    {
        public long ISMS_Id { get; set; }
        public Exm_Yrly_Cat_Exams_Subwise_SubExamsDTO[] sub_exam_list { get; set; }
    }
    public class tempDTO_subsubjects
    {
        public long ISMS_Id { get; set; }
        public Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDTO[] sub_subjs_list { get; set; }
    }
    public class tempDTO_subsubjects_subexam
    {
        public long ISMS_Id { get; set; }
        public Exm_Yrly_Cat_Exams_Subwise_SubSubjects_subexamDTO[] sub_subject_sub_exam_list { get; set; }
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
    public class Exm_Yrly_Cat_Exams_SubwiseDTO : CommonParamDTO
    {
        public int EYCES_Id { get; set; }
        public int EYCE_Id { get; set; }
        public long ISMS_Id { get; set; }
        public int? EMGR_Id { get; set; }
        public decimal? EYCES_MarksEntryMax { get; set; }
        public decimal? EYCES_MaxMarks { get; set; }
        public decimal? EYCES_MinMarks { get; set; }
        public bool EYCES_SubExamFlg { get; set; }
        public bool EYCES_SubSubjectFlg { get; set; }
        public string EYCES_MarksGradeEntryFlg { get; set; }
        public bool EYCES_MarksDisplayFlg { get; set; }
        public bool EYCES_GradeDisplayFlg { get; set; }
        public bool EYCES_AplResultFlg { get; set; }
        public bool EYCES_ActiveFlg { get; set; }
        public int EYCES_SubjectOrder { get; set; }
        public Exam_Subject_PT_GradeList[] Exam_Subject_PT_GradeList { get; set; }
    }
    public class Exm_Yrly_Cat_Exams_Subwise_SubExamsDTO : CommonParamDTO
    {
        public int EYCESSE_Id { get; set; }
        public int EYCES_Id { get; set; }
        public int EMSE_Id { get; set; }
        public int EMGR_Id { get; set; }
        public decimal? EYCESSE_MaxMarks { get; set; }
        public decimal? EYCESSE_MinMarks { get; set; }
        public decimal? EYCESSS_MarksEntryMax { get; set; }
        public bool EYCESSE_ExemptedFlg { get; set; }
        public decimal? EYCESSE_ExemptedPer { get; set; }
        public bool EYCESSE_ActiveFlg { get; set; }
        public int EYCESSE_SubExamOrder { get; set; }
        public bool EYCESSS_MarksFlg { get; set; }
        public bool EYCESSS_GradesFlg { get; set; }
        public bool EYCESSS_AplResultFlg { get; set; }
    }
    public class Exm_Yrly_Cat_Exams_Subwise_SubSubjectsDTO : CommonParamDTO
    {
        public int EYCESSS_Id { get; set; }
        public int EYCES_Id { get; set; }
        public int EMSS_Id { get; set; }
        public int EMGR_Id { get; set; }
        public decimal? EYCESSS_MaxMarks { get; set; }
        public decimal? EYCESSS_MinMarks { get; set; }
        public decimal? EYCESSS_MarksEntryMax { get; set; }
        public bool EYCESSS_ExemptedFlg { get; set; }
        public decimal? EYCESSS_ExemptedPer { get; set; }
        public bool EYCESSS_ActiveFlg { get; set; }
        public int EYCESSS_SubSubjectOrder { get; set; }
        public bool EYCESSS_MarksFlg { get; set; }
        public bool EYCESSS_GradesFlg { get; set; }
        public bool EYCESSS_AplResultFlg { get; set; }
    }
    public class Exm_Yrly_Cat_Exams_Subwise_SubSubjects_subexamDTO : CommonParamDTO
    {
        public int EYCESSS_Id { get; set; }
        public int EYCES_Id { get; set; }
        public int EMSS_Id { get; set; }
        public int EMSE_Id { get; set; }
        public int EMGR_Id { get; set; }
        public decimal? EYCESSS_MaxMarks { get; set; }
        public decimal? EYCESSS_MinMarks { get; set; }
        public decimal? EYCESSS_MarksEntryMax { get; set; }
        public bool EYCESSS_ExemptedFlg { get; set; }
        public decimal? EYCESSS_ExemptedPer { get; set; }
        public bool EYCESSS_ActiveFlg { get; set; }
        public int EYCESSS_SubSubjectOrder { get; set; }
        public bool EYCESSS_MarksFlg { get; set; }
        public bool EYCESSS_GradesFlg { get; set; }
        public bool EYCESSS_AplResultFlg { get; set; }
    }
    public class Temp_Subject_Order
    {
        public long EYCES_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long EYCE_Id { get; set; }
        public int EYCES_SubjectOrder { get; set; }
    }
    public class Exam_Subject_PT_GradeList
    {
        public int EYCESPT_Id { get; set; }
        public int EYCES_Id { get; set; }
        public int EMGR_Id { get; set; }
        public int EMPATY_Id { get; set; }        
        public long ISMS_Id { get; set; }
    }
}