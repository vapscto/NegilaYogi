using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Exam.StudentMentorMapping
{
    public class CollegedepartmentcoursebranchmappingDTO
    {
        public long ADCOBS_Id { get; set; }
        public long ADCO_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long Userid { get; set; }
        public long HRMD_Id { get; set; }
        public int HRMD_Order { get; set; }
        public int AMCO_Order { get; set; }
        public int AMB_Order { get; set; }
        public int AMSE_Order { get; set; }
        public string HRMD_DepartmentName { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMB_BranchName { get; set; }
        public string AMSE_SEMName { get; set; }
        public Array deptlist { get; set; }
        public Array courselist { get; set; }
        public Array branchlist { get; set; }
        public Array semesterlist { get; set; }
        public Array getdetails { get; set; }
        public Array getsemdetails { get; set; }
        public Array getreport { get; set; }
        public bool returnval { get;set;}
        public bool ADCO_ActiveFlag { get; set; }
        public bool ADCOBS_ActiveFlag { get; set; }
        public string message { get; set; }
        public CollegedepartmentcoursebranchmappingTempDTO[] semesterselecteddetails { get; set; }

    }
    public class CollegedepartmentcoursebranchmappingTempDTO
    {
        public long AMSE_Id { get; set; }
        public string AMSE_SEMName { get; set; }
    }
}
