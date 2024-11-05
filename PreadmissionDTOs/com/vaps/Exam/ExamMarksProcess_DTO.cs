using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.Exam
{
   public class ExamMarksProcess_DTO:CommonParamDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public int EYC_Id { get; set; }        
        public int EYCE_Id { get; set; }       
        public int EME_Id { get; set; }

        public DateTime? EYCE_MindateDate { get; set; }

        public DateTime? EYCE_ExamStartDate { get; set; }
        public DateTime? EYCE_ExamEndDate { get; set; }
        public DateTime? EYCE_MarksEntryLastDate { get; set; }
        public DateTime? EYCE_MarksProcessLastDate { get; set; }
        public DateTime? EYCE_MarksPublishDate { get; set; }
        public Array yearlist { get; set; }
        public Array categorylist { get; set; }
        public Array subjectlist { get; set; }
        public Array examlist { get; set; }
        public Array category_exams { get; set; }
        public Array edit_cat_exm { get; set; }
        public Array edit_cat_exm_subs { get; set; }
        public Array edit_cat_exm_subs_sub_subjs { get; set; }
        public Array examlistdetails { get; set; }
        public Array view_exam_subjects { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }
        public int EMCA_Id { get; set; }
        public string ASMAY_Year { get; set; }
        public string EMCA_CategoryName { get; set; }
        public string EME_ExamName { get; set; }
        public string EME_ExamCode { get; set; }
        public string EMGR_GradeName { get; set; }
        public DateTime? EYCE_AttendanceFromDate { get; set; }
        public DateTime? EYCE_AttendanceToDate { get; set; }
        public bool EYCE_SubExamFlg { get; set; }
        public bool EYCE_SubSubjectFlg { get; set; }
        public bool EYCE_ActiveFlg { get; set; }
        public bool? EYCESU_MarksPublishApproverFlg { get; set; }
        public long ISMS_Id { get; set; }
        public string ISMS_SubjectName { get; set; }
        public string ISMS_SubjectCode { get; set; }
        public decimal? ISMS_Max_Marks { get; set; }
        public decimal? ISMS_Min_Marks { get; set; }
        public long ISMS_OrderFlag { get; set; }
        public int EMGR_Id { get; set; }
        public int EYCES_Id { get; set; }
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



        // User Promotion

        public long EYCESU_Id { get; set; }
        public long IVRMULF_Id { get; set; }

        public long UserId { get; set; }
        public string HRME_EmployeeFirstName { get; set; }
        public long Emp_Code { get; set; }
        public Array userlist { get; set; }
        public DateTime? EYCESU_MarksEntryFromDate { get; set; }
        public DateTime? EYCESU_MarksEntryToDate { get; set; }
        public DateTime? EYCESU_MarksProcessFromDate { get; set; }
        public DateTime? EYCESU_MarksProcessToDate { get; set; }
        public DateTime? EYCESU_MarksPublishDate { get; set; }
        public Array userPromotionlist { get; set; }
        public Array editPromotionUserlist { get; set; }
        public bool EYCESU_ActiveFlg { get; set; }
        public ivrmulF_IdList[] ivrmulF_IdList { get; set; }
        public int dulicateCount { get; set; }
        public int savedcount { get; set; }
    }
    public class ivrmulF_IdList
    {
        public long IVRMULF_Id { get; set; }
    }

}
