using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Exam
{
    public class ClgMarksEntryDTO
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
        public string amcsT_RegistrationNo { get; set; }
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
        public Array getschemetype { get; set; }
        public Array getsubjectschemetype { get; set; }
        public Array configuration { get; set; }
        public marksbranch[] marksbranch { get; set; }
        public markssection[] markssection { get; set; }
        public long? courseid { get; set; }
        public long? semesterid { get; set; }
        public long? branchid { get; set; }
        public long? sectionid { get; set; }
    }
    public class marksDTO
    {
        public long amcsT_Id { get; set; }
        public decimal? totalMarks { get; set; }
        public decimal? minMarks { get; set; }
        public string obtainmarks { get; set; }      
        public long? courseid { get; set; }
        public long? semesterid { get; set; }
        public long? branchid { get; set; }
        public long? sectionid { get; set; }        
    }

    public class marksbranch
    {
        public long AMB_Id { get; set; }
    }
    public class markssection
    {
        public long ACMS_Id { get; set; }
    }
}
