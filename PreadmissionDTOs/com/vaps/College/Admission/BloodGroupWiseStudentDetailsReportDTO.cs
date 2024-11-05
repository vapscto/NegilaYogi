using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class BloodGroupWiseStudentDetailsReportDTO
    {
        public long MI_Id { get; set; }
        public Array allacademicyear { get; set; }
        public Array courselist { get; set; }
        public Array branchlist { get; set; }
        public Array semesterlist { get; set; }
        public Array sectionlist { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public Array studentDetails { get; set; }
        public string ASMAY_Year { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMB_BranchName { get; set; }
        public string AMSE_SEMName { get; set; }
        public string ACMS_SectionName { get; set; }
        public BloodGroupWiseStudentDetailsReportDTO[] branchess { get; set; }
        public BloodGroupWiseStudentDetailsReportDTO[] blood1 { get; set; }
        public clsid[] clsidlist { get; set; }
        public Array all { get; set; }
        public Array year { get; set; }
        public Array sem { get; set; }

    }
    public class clsid
    {
        public long ACMS_Id { get; set; }
    }



}
