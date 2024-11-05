using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class CollegeDailyAttendanceDTO
    {

        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long ISMS_Id { get; set; }
        public long ISMS_order { get; set; }
        public long IVRM_Month_Id { get; set; }
        public string username { get; set; }
        public long userId { get; set; }
        public string Flag1 { get; set; }
        public long roleId { get; set; }
        public long ACYST_RollNo { get; set; }
        public string AMB_BranchName { get; set; }
        public string ISMS_SubjectName { get; set; }
        public int AMB_Order { get; set; }
        public Array allyear1 { get; set; }

        public string AMSE_SEMName { get; set; }
        public Array studentAbsent_teacherList { get; set; }

        

        public Array acdlist { get; set; }
        public Array courselist { get; set; }
        public Array branchlist { get; set; }
        public Array semlist { get; set; }
        public Array seclist { get; set; }
        public Array studentreport { get; set; }
        public Array subjectlist { get; set; }
        public Array datelist { get; set; }
        public Array monthlist { get; set; }
        public Temp_branchDTO[] Temp_branchDTO { get; set; }
        public Temp_subjectDTO[] Temp_subjectDTO { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Flag { get; set; }
        public AbsentManualsms[] absentlist { get; set; }
        public string message { get; set; }
        public string shortage { get; set; }
        public string rolename { get; set; }
        public long Emp_Code { get; set; }     
        public Array getstudentlist { get; set; }
        public Array CollegestudentAttendanceList { get; set; }
       
        public string studentname { get; set; }

        public string Todates { get; set; }

    }

    public class Temp_branchDTO
    {
        public long AMB_Id { get; set; }
        public string AMB_BranchName { get; set; }
    }
    public class Temp_subjectDTO
    {
        public long ISMS_Id { get; set; }
        public string ISMS_SubjectName { get; set; }
    }
    public class AbsentManualsms
    {
        public long AMCST_Id { get; set; }
        public string AMCST_FirstName { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMB_BranchName { get; set; }
        public string AMSE_SEMName { get; set; }
        public long AMCST_MobileNo { get; set; }
        public string subject { get; set; }
    }
}
