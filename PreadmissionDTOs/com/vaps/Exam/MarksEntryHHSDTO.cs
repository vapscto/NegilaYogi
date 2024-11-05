using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class MarksEntryHHSDTO : CommonParamDTO
    {
        public long MI_Id { set; get; }
        public long UserId { get; set; }
        public long roleid { get; set; }
        public long EMSS_Id { get; set; }
        public Array yearlist { get; set; }
        public long ASMAY_Id { set; get; }
        public Array classlist { get; set; }
        public long ASMCL_Id { set; get; }
        public Array sectionlist { get; set; }
        public long ASMS_Id { set; get; }
        public Array examlist { get; set; }
        public int EME_Id { set; get; }
        public Array subjectlist { get; set; }
        public long ISMS_Id { set; get; }
        public Array subject_details { get; set; }
        public string EYCES_MarksGradeEntryFlg { get; set; }
        public bool EYCES_SubSubjectFlg { get; set; }
        public Array subject_subsubjects { get; set; }
        public bool EYCES_SubExamFlg { get; set; }
        public Array subject_subexams { get; set; }
        public int EMGR_Id { get; set; }
        public Array grade_details { get; set; }
        public long AMST_Id { set; get; }
        public string AMST_FirstName { get; set; }
        public string AMST_AdmNo { get; set; }
        public long AMAY_RollNo { get; set; }
        public string ISMS_SubjectName { get; set; }
        public decimal? EYCES_MaxMarks { get; set; }
        public decimal? EYCES_MinMarks { get; set; }
        public decimal? EYCES_MarksEntryMax { get; set; }
        public decimal ESTM_Marks { get; set; }
        public string ESTM_Flg { get; set; }
        public Array studentList { get; set; }
        public Array get_student_wise_papertype_list { get; set; }
        public Array subsubject_gradedetails { get; set; }
        public Array subexam_gradedetails { get; set; }
        public string IP4 { get; set; }
        public temp_marks_DTO[] main_save_list { get; set; }
        public Temp_subs_marks_DTO[] Temp_subs_marks_list { get; set; }
        public bool returnval { get; set; }
        public Array saved_studentList { get; set; }
        public bool marksdeleteflag { get; set; }
        public Array saved_ssse_list { get; set; }
        public Array saved_ss_list { get; set; }
        public Array saved_se_list { get; set; }
        public Array configuration { get; set; }
        public string orderflag { get; set; }
        public Array subsubjectlist { get; set; }
        public Array subexamlist { get; set; }
        public Array subsubjectsubexamlist { get; set; }
        public long saveupdatecount { get; set; }
        public long lastdateentry { get; set; }
        public bool lastdateentryflag { get; set; }
        public long lastdateexam { get; set; }
        public bool lastdateexamflag { get; set; }
        public DateTime? marksentrystatedate { get; set; }
        public string mobileprivileges { get; set; }
        public string stringmobileorportal { get; set; }
        public string Pagename { get; set; }
        public string Pageicon { get; set; }
        public string Pageurl { get; set; }
        public long? IVRMRMAP_Id { get; set; }
        public bool? IVRMMAP_AddFlg { get; set; }
        public bool? IVRMMAP_UpdateFlg { get; set; }
        public bool? IVRMMAP_DeleteFlg { get; set; }
        public Array Staffmobileappprivileges { get; set; }
        public Array get_papertype_grade_details { get; set; }
        public int EMPATY_Id { get; set; }
        public int EMGD_Id { get; set; }
        public string EMPATY_PaperTypeName { get; set; }
        public string EMGR_GradeName { get; set; }
        public string EMGD_Name { get; set; }
    }
}