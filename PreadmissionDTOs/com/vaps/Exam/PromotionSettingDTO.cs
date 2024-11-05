using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class PromotionSettingDTO : CommonParamDTO
    {
        //master promotion
        public int EMP_Id { get; set; }
        public long MI_Id { get; set; }
        public int EYC_Id { get; set; }
        public int EMGR_Id { get; set; }
        public bool EMP_PassToIndSubjectFlg { get; set; }
        public bool EMP_PassToOverallFlag { get; set; }
        public string EMP_MarksPerFlg { get; set; }
        public bool EMP_ActiveFlag { get; set; }
        //prom subjs
        public int EMPS_Id { get; set; }
        public long ISMS_Id { get; set; }
        public decimal? EMPS_MaxMarks { get; set; }
        public decimal? EMPS_MinMarks { get; set; }
        public decimal? EMPS_ConvertForMarks { get; set; }
        public bool EMPS_AppToResultFlg { get; set; }
        public bool EMPS_ActiveFlag { get; set; }
        //prom subj_group
        public int EMPSG_Id { get; set; }
        public string EMPSG_GroupName { get; set; }
        public string EMPSG_DisplayName { get; set; }
        public decimal? EMPSG_PercentValue { get; set; }
        public decimal? EMPSG_MarksValue { get; set; }
        public int EMPSG_MaxOff { get; set; }
        public int EMPSG_BestOff { get; set; }
        public bool EMPSG_ActiveFlag { get; set; }
        //prom subj_group_exms
        public int EMPSGE_Id { get; set; }
        public int EME_Id { get; set; }
        public bool EMPSGE_ActiveFlg { get; set; }
        public int EMCA_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string ASMAY_Year { get; set; }
        public string EMCA_CategoryName { get; set; }
        public string EME_ExamName { get; set; }
        public string EME_ExamCode { get; set; }
        public string EMGR_GradeName { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string ISMS_SubjectCode { get; set; }
        public decimal? ISMS_Max_Marks { get; set; }
        public decimal? ISMS_Min_Marks { get; set; }
        public long ISMS_OrderFlag { get; set; }
        public string EMSE_SubExamName { get; set; }
        public string EMSE_SubExamCode { get; set; }
        public string EMSS_SubSubjectName { get; set; }
        public string EMSS_SubSubjectCode { get; set; }
        public string returnduplicatestatus { get; set; }
        public bool returnval { get; set; }
        public Array exm_prom_groups { get; set; }
        public Array yearlist { get; set; }
        public Array categorylist { get; set; }
        public Array gradelist { get; set; }
        public Array examlist { get; set; }
        public Array subexamlist { get; set; }
        public Array subjectlist { get; set; }
        public Array subsubjectlist { get; set; }
        public Array GetYearlyExamSubjectMarks { get; set; }
        public Exm_M_Promotion_SubjectsDTO[] pro_subjects_list { get; set; }
        public Exm_M_Prom_Subj_GroupDTO[] pro_exams_group_list {get;set;}
        public Array promotion_details { get; set; }
        public Array view_prom_subjects { get; set; }
        public Array view_exam_subjects_subgroup_exms { get; set; }
        public Array view_exam_subjects_subgroups { get; set; }
        public Array edit_m_pro { get; set; }
        public Array edit_m_pro_subs { get; set; }
        public Array edit_m_pro_subs_grps { get; set; }
        public Array edit_m_pro_subs_grps_exms { get; set; }

        //master exam remove later
        public int EME_ExamOrder { get; set; }
        public int ASMAY_Order { get; set; }
        public bool EME_FinalExamFlag { get; set; }
        public bool EME_ActiveFlag { get; set; }      
        public bool Calculated_Flag { get; set; }
        public int? EMPSG_Order { get; set; }
        public int? EMPS_SubjOrder { get; set; }
        public bool? EMP_BestOfApplicableFlg { get; set; }
        public long? EMP_BestOf { get; set; }
        public decimal? EMPSGE_ForMaxMarkrs { get; set; }
        public decimal? EYCES_MaxMarks { get; set; }
        public bool? EMPSGE_ConvertionReqOrNot { get; set; }
        public bool? EMPSG_RoundOffFlag { get; set; }
        public subject_list_temp[] subject_list_temp { get; set; }
    }

    public class Exm_M_Prom_Subj_GroupDTO : CommonParamDTO
    {
        public int EMPSG_Id { get; set; }
        public int EMPS_Id { get; set; }
        public string EMPSG_GroupName { get; set; }
        public string EMPSG_DisplayName { get; set; }
        public decimal? EMPSG_PercentValue { get; set; }
        public decimal? EMPSG_MarksValue { get; set; }
        public int EMPSG_MaxOff { get; set; }
        public int EMPSG_BestOff { get; set; }
        public bool EMPSG_ActiveFlag { get; set; }
        public int? EMPSG_Order { get; set; }
        public bool? EMPSG_RoundOffFlag { get; set; }
        public Exm_Master_ExampromtoionDTO[] Exm_M_Prom_Subj_Group_Exams_master { get; set; }
    }

    public class Exm_M_Promotion_SubjectsDTO : CommonParamDTO
    {
        public int EMPS_Id { get; set; }
        public int EMP_Id { get; set; }
        public long ISMS_Id { get; set; }
        public int EMGR_Id { get; set; }
        public decimal? EMPS_MaxMarks { get; set; }
        public decimal? EMPS_MinMarks { get; set; }
        public decimal? EMPS_ConvertForMarks { get; set; }
        public bool EMPS_AppToResultFlg { get; set; }
        public bool EMPS_ActiveFlag { get; set; }
        public int? EMPS_SubjOrder { get; set; }
        public Exm_M_Prom_Subj_GroupDTO[] pro_exams_group_list { get; set; }

    }
    public class Exm_Master_ExampromtoionDTO : CommonParamDTO
    {
        public int EME_Id { get; set; }
        public long MI_Id { get; set; }
        public string EME_ExamName { get; set; }
        public string EME_ExamCode { get; set; }
        public int EME_ExamOrder { get; set; }
        public bool EME_FinalExamFlag { get; set; }
        public bool EME_ActiveFlag { get; set; }
        public bool? EMPSGE_ConvertionReqOrNot { get; set; }
        public decimal? EMPSGE_ForMaxMarkrs { get; set; }
    }

    public class subject_list_temp
    {
        public long ISMS_Id { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string ISMS_SubjectCode { get; set; }
        public long EMPS_Id { get; set; }
        public int? EMPS_SubjOrder { get; set; }
    }
}
