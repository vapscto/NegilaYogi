using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Portals.Student
{
    public class ClgPortalAttendanceDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public string ASMAY_Year { get; set; }
        public long AMCO_Id { get; set; }
        public long ACMS_Id { get; set; }
        public string ACMS_SectionName { get; set; }
        public long AMCST_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public Array yearlist { get; set; }
        public Array currentyear { get; set; }
        public Array attList { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMSE_SEMName { get; set; }
        public string AMB_BranchName { get; set; }
        public string AMB_BranchCode { get; set; }

        //============================================== Staff Student Attendance
        public long UserId { get; set; }
        public long HRME_Id { get; set; }
        public string AMCO_CourseCode { get; set; }
        public bool AMCO_CourseFlag { get; set; }
        public bool AMCO_ActiveFlag { get; set; }
        public int AMCO_Order { get; set; }

        public int AMB_Order { get; set; }
        public bool AMB_ActiveFlag { get; set; }
        public string AMSE_SEMCode { get; set; }
        public int AMSE_SEMOrder { get; set; }

        public string AMCST_FirstName { get; set; }
        public string AMCST_MiddleName { get; set; }
        public string AMCST_LastName { get; set; }


        public Array course_list { get; set; }
        public Array branch_list { get; set; }
        public Array sem_list { get; set; }
        public Array attendancereport { get; set; }
        public Array get_studentsearch { get; set; }
        
        public Array student_list { get; set; }

        public BranchArrayDTO[] branchArray { get; set; }
        public SemesterArrayDTO[] semesterArray { get; set; }

    }
    public class BranchArrayDTO
    {
        public long INTBCB_Id { get; set; }
        public long AMB_Id { get; set; }
        public string AMB_BranchName { get; set; }
    }
    public class SemesterArrayDTO
    {
        public long INTBCB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public string AMSE_SEMName { get; set; }
    }


}
