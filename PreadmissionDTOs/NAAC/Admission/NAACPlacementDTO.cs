using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.NAAC.Admission
{
    public class NAACPlacementDTO
    {
        public long NCAC521PLA_Id { get; set; }
        public long NCAC521PLAF_Id { get; set; }
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long NCAC521PLA_Year { get; set; }
        public long NCAC521PLA_NoOfstudentsselfemployed { get; set; }
        public long NCAC521PLA_NoOfStudents { get; set; }
        public string NCAC521PLA_EmployerName { get; set; }
        public string AMCO_CourseName { get; set; }
        public string NCAC521PLA_StatusFlg { get; set; }
        public string  AMB_BranchName { get; set; }
        public string NCAC521PLA_Package { get; set; }
        public long NCAC521PLA_GradCourse { get; set; }
        public long NCAC521PLA_GradBranch { get; set; }
        public bool NCAC521PLA_ActiveFlg { get; set; }
        public long NCAC521PLA_CreatedBy { get; set; }
        public long NCAC521PLA_UpdatedBy { get; set; }
        public DateTime NCAC521PLA_CreatedDate { get; set; }
        public DateTime NCAC521PLA_UpdatedDate { get; set; }
        public long UserId { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public Array institutionlist { get; set; }
        public Array branchlist { get; set; }
        public Array courselist { get; set; }

        public string ASMAY_Year { get; set; }

        public Array allacademicyear { get; set; }
        public Array alldatalist { get; set; }
        public Array editlist { get; set; }
        public bool duplicate { get; set; }
        public bool returnval { get; set; }

        public Array editfiles { get; set; }

        public NAACCriteriaFivefileDTO[] filelist { get; set; }
        public string Remarks { get; set; }
        public long filefkid { get; set; }
        public Array commentlist { get; set; }
    }
}
