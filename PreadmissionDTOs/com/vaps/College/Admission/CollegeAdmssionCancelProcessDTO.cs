using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class CollegeAdmssionCancelProcessDTO
    {
        public long MI_Id { get; set; }
        public long userid { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long ACMS_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long AMSE_Id { get; set; }
        public int count { get; set; }
        public int todays { get; set; }
        public Array yearlist { get; set; }
        public Array courselist { get; set; }
        public Array branchlist { get; set; }
        public Array semesterlist { get; set; }
        public Array studentlist { get; set; }
        public Array studentdetails { get; set; }
        public Array sectionlist { get; set; }
        public Array cancelconfigurationdetails { get; set; }
        public string message { get; set; }
        public string studentName { get; set; }
        public string AMCST_Admno { get; set; }
        public string AMB_BranchName { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMSE_SEMName { get; set; }
        public string ACMS_SectionName { get; set; }
        public string cancellationtype { get; set; }
        public string reason { get; set; }
        public bool returnval { get; set; }
        public DateTime opendate { get; set; }
        public DateTime AMCST_Date { get; set; }
        public decimal refundper { get; set; }
        public decimal cancelper { get; set; }



    }
}
