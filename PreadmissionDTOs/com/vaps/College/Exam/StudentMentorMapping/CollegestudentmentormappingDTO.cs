using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Exam.StudentMentorMapping
{
    public class CollegestudentmentormappingDTO
    {
        public long AMMEC_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long HRME_Id { get; set; }
        public bool ECSMU_Activeflag { get; set; }
        public long ECSMU_CreatedBy { get; set; }
        public long ECSMU_UpdatedBy { get; set; }
        public long AMMECM_Id { get; set; }        
        public long AMCST_Id { get; set; }
        public bool ECSMD_Activeflag { get; set; }
        public long ECSMD_CreatedBy { get; set; }
        public long ECSMD_UpdatedBy { get; set; }
        public long Userid { get; set; }
        public Array getyear { get; set; }
        public Array getcourse { get; set; }
        public Array getbranch { get; set; }
        public Array getsemester { get; set; }
        public Array getsection { get; set; }
        public Array getstudentlist { get; set; }
        public Array getsavedstudentlist { get; set; }
        public Array getemployeedetails { get; set; }
        public string message { get; set; }
        public bool returnval { get; set; }
        public Array viewdata { get; set; }
        public Array getdetails { get; set; }
        public string coursename { get; set; }
        public string branchname { get; set; }
        public string semestername { get; set; }
        public string sectioname { get; set; }
        public string employeename { get; set; }
        public string yearname { get; set; }
        public CollegestudentmentormappingtempDTO[] CollegestudentmentormappingtempDTO { get; set; }
        public Array getstudentdata { get; set; }
        public Array getreportdata { get; set; }
    }

    public class CollegestudentmentormappingtempDTO
    {
        public long AMCST_Id { get; set; }
        public string studentname { get; set; }

    }
}
