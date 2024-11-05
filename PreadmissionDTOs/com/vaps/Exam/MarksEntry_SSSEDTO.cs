using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class MarksEntry_SSSEDTO : CommonParamDTO
    {
        public long MI_Id { set; get; }
        public long UserId { get; set; }
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
        public Array subsubject_gradedetails { get; set; }
        public Array subexam_gradedetails { get; set; }
        public string IP4 { get; set; }

        public temp_marks_DTO[] main_save_list { get; set; }
        public Temp_subs_marks_DTO[] Temp_subs_marks_list { get; set; }
        public bool returnval { get; set; }
        public Array saved_studentList { get; set; }
        public Array saved_ssse_list { get; set; }
        public bool marksdeleteflag { get; set; }
        //public string subMorGFlag { get; set; }
        //public long EMGR_Id { get; set; }
        //public Array gradname { get; set; }


    }
    public class temp_marks_DTO
    {
        public long AMST_Id { set; get; }
        public string AMST_FirstName { get; set; }
        public string AMST_AdmNo { get; set; }
        public string AMST_RegistrationNo { get; set; }
        public long AMAY_RollNo { get; set; }
        public long ISMS_Id { set; get; }
        public string ISMS_SubjectName { get; set; }
        public decimal? EYCES_MaxMarks { get; set; }
        public decimal? EYCES_MinMarks { get; set; }
        public decimal? EYCES_MarksEntryMax { get; set; }
        public decimal ESTM_Marks { get; set; }
        public string ESTM_Grade { get; set; }
        public string ESTM_Flg { get; set; }
        public int ESTM_Id { get; set; }
        public string param { get; set; }
        public string AMST_Sex { get; set; }
    }
    public class Temp_subs_marks_DTO
    {
        public int ESTMSS_Id { get; set; }
        //  public long MI_Id { get; set; }
        //  public int ESTM_Id { get; set; }
        public int AMST_Id { get; set; }
        public int EMSS_Id { get; set; }
        public long ISMS_Id { get; set; }
        public int EMSE_Id { get; set; }
        public decimal? ESTMSS_Marks { get; set; }
        public string ESTMSS_MarksGradeFlg { get; set; }
        public string ESTMSS_Grade { get; set; }
       // public long Login_Id { get; set; }
      //  public DateTime LoginDateTime { get; set; }
     //   public string IP4 { get; set; }
        public string ESTMSS_Flg { get; set; }
        public bool ESTMSS_ActiveFlg { get; set; }
    }
}
