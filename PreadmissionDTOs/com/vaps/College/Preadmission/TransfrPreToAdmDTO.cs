using PreadmissionDTOs.com.vaps.admission;
using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Preadmission
{
   public class TransfrPreToAdmDTO
    {
        public long MI_Id { get; set; }
        public long roleId { get; set; }
        public long LogInUserId { get; set; }
        public long userid { get; set; }
        public string Name { get; set; }
        public long ASMAY_Id { get; set; }
        public Array courselist { get; set; }
        public Array branchlist { get; set; }
        public Array semesterlist { get; set; }
        public Array yearlist { get; set; }
        public long AMB_Id { get; set; }
        public string AMB_BranchName { get; set; }
        public string AMCO_CourseName { get; set; }
        public long AMSE_Id { get; set; }
        public long PACA_Id { get; set; }
      
        public string AMSE_SEMName { get; set; }
        public string studentname { get; set; }
        public courselistarray1[] courselistarray { get; set; }
        public branchlistarray1[] branchlistarray { get; set; }
        public semesterlistarray1[] semesterlistarray { get; set; }

        public AMCST_IDarray[] AMCST_IDarray { get; set; }

        public long AMCST_IDuser { get; set; }

        public Array preAdmtoAdmStuList { get; set; }
        public int payementcheck { get; set; }
        public List<TransfrPreToAdmDTO> studentdetails { get; set; }
        public MasterConfigurationDTO configurationsettings { get; set; }
        public CollegeUsernameCreationDTO CollegeUsernameCreationDTO { get; set; }
        

        public string returnMsg { get; set; }

        public bool returnval { get; set; } = true;

        public long AMCST_MobileNo { get; set; }

        public string AMCST_emailId { get; set; }

        //username creation
        public string msg { get; set; }
        public long? AMCST_FatherMobleNo { get; set; }
        public long? AMCST_MotherMobleNo { get; set; }
        public string Studenttype { get; set; }
        public Temp_Student[] Temp_Student { get; set; }
        public Temp_Student_SMS[] Temp_Student_SMS { get; set; }

    }
    public class AMCST_IDarray
    {
        public long AMCST_Id { get; set; }
    }
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
    public class courselistarray1
    {
        public long AMCO_Id { get; set; } 
    }
    public class branchlistarray1
    {
        public long AMB_Id { get; set; }
    }
    public class semesterlistarray1
    {
        public long AMSE_Id { get; set; }
    }
}
