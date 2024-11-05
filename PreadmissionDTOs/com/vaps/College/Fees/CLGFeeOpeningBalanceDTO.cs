using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreadmissionDTOs.com.vaps.College.Fees
{
    public class CLGFeeOpeningBalanceDTO
    {
        public long MI_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public Array yearlist { get; set; }
        public Array courselist { get; set; }
        public Array branchlist { get; set; }
        public Array semisterlist { get; set; }
        public Array sectionlist { get; set; }
        public Array saveddata { get; set; }
        public bool returnval { get; set; }
        public bool returnduplicatestatus { get; set; }
        public string ACMAY_AcademicYear { get; set; }
        public string AMCO_CourseName { get; set; }
        public string AMB_BranchName { get; set; }
        public string AMSE_SEMName { get; set; }
        public string ACMS_SectionName { get; set; }
        public long UserId { get; set; }
        public Array grouplist { get; set; }
        public long FMG_Id { get; set; }
        public string FMG_GroupName { get; set; }
        public Array headlist { get; set; }
        public long FMH_Id { get; set; }
        public string FMH_FeeName { get; set; }
        public Array installmentlist { get; set; }
        public long FTI_Id { get; set; }
        public string FTI_Name { get; set; }
        public Array studentlist { get; set; }
        public Temp_Save_DTO[] sub_data { get; set; }
    }
    public class Temp_Save_DTO
    {
        public long AMCST_Id { get; set; }
        public decimal FCMOB_Student_Due { get; set; }
        public decimal FCMOB_Institution_Due { get; set; }
    }
}
