using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class CollegeUsernameCreationDTO
    {
        public long MI_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long? AMCST_FatherMobleNo { get; set; }
        public long? AMCST_MotherMobleNo { get; set; }
        public long? AMCST_MobileNo { get; set; }
        public long Userid { get; set; }
        public Array yearlist { get; set; }
        public Array courselist { get; set; }
        public Array branchlist { get; set; }
        public Array semesterlist { get; set; }
        public Array sectionlist { get; set; }
        public Array studentlist { get; set; }
        public Array studentuserdetails { get; set; }
        public string MI_Logo { get; set; }
        public string Studenttype { get; set; }
        public string AMCST_Admno { get; set; }
        public string studentName { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMB_BranchName { get; set; }
        public string AMSE_SEMName { get; set; }
        public string ACMS_SectionName { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string AMCST_emailId { get; set; }
        public string smsmsg { get; set; }
        public string emailmsg { get; set; }
        public string msg { get; set; }
        public Temp_Student[] Temp_Student { get; set; }
        public Temp_Student_SMS[] Temp_Student_SMS { get; set; }
        public bool SMSFlag { get; set; }
        public bool EmailFlag { get; set; }


    }
    public class Temp_Student
    {
        public long AMCST_Id { get; set; }
        public string studentName { get; set; }
    }
    public class Temp_Student_SMS
    {
        public long AMCST_Id { get; set; }
        public string studentName { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string AMCST_emailId { get; set; }
        public long? AMCST_MobileNo { get; set; }

    }
}
