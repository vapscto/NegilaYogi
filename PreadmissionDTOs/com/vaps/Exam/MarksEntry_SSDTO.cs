using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.Exam
{
    public class MarksEntry_SSDTO : CommonParamDTO
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
        public Array saved_ss_list { get; set; }
        public bool marksdeleteflag { get; set; }
        //public string subMorGFlag { get; set; }
        //public long EMGR_Id { get; set; }
        //public Array gradname { get; set; }


    }
  
}
