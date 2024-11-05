using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class ExamMarksDTO : CommonParamDTO
    {
        public int ESTM_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long ASMCL_Id { get; set; }
        public long ASMS_Id { get; set; }
        public long AMST_Id { get; set; }
        public int EME_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long roleid { get; set; }
        public decimal ESTM_Marks { get; set; }
        public string ESTM_MarksGradeFlg { get; set; }
        public long Id { get; set; }
        public DateTime LoginDateTime { get; set; }
        public string IP4 { get; set; }
        public bool ESTM_ActiveFlg { get; set; }
        public string ESTM_Grade { get; set; }
        public string ESTM_Flg { get; set; }
        public Array ctlist { get; set; }
        public Array seclist { get; set; }
        public Array subjectlist { get; set; }
        public Array Acdlist { get; set; }
        public Array examlist { get; set; }
        public Array configuration { get; set; }
        public Array studentList { get; set; }
        public string SubjectName { get; set; }
        public decimal? TotalMarks { get; set; }
        public decimal? MarksEnterFor { get; set; }
        public decimal? MinMarks { get; set; }
        public string obtainmarks { get; set; }
        public string obtainvaluesold { get; set; }
        public long? amaY_RollNo { get; set; }
        public string amsT_AdmNo { get; set; }
        public string amsT_RegistrationNo { get; set; }
        public string studentname { get; set; }
        public marksDTO[] detailsList { get; set; }
        public string messagesaveupdate { get; set; }
        public string subMorGFlag { get; set; }
        public int EMGR_Id { get; set; }
        public Array gradname { get; set; }
        public string grade { get; set; }
        public string marksdeleteflag { get; set; }
        public long saveupdatecount { get; set; }
        public long lastdateentry { get; set; }
        public bool lastdateentryflag { get; set; }
        public Array subsubjectlist { get; set; }
        public Array subexamlist { get; set; }
        public int EMSS_Id { get; set; }
        public int EMSE_Id { get; set; }
        public long lastdateexam { get; set; }
        public bool lastdateexamflag { get; set; }
        public DateTime? marksentrystatedate { get; set; }
        public string messagesub { get; set; }
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
        public Array get_student_wise_papertype_list { get; set; }
        public Array get_papertype_grade_details { get; set; }
    }
    public class marksDTO
    {
        public long amsT_Id { get; set; }
        public int estM_Id { get; set; }
        public decimal? totalMarks { get; set; }
        public decimal? minMarks { get; set; }
        public string obtainmarks { get; set; }
    }
}
