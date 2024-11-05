using System;
using System.Collections.Generic;
using System.Text;

namespace PreadmissionDTOs.com.vaps.College.Admission
{
    public class CollegeActiveDeactiveStudentsDTO
    {
        public long ACSDE_Id { get; set; }
        public long MI_Id { get; set; }
        public long AMCST_Id { get; set; }
        public long ASMAY_Id { get; set; }
        public long AMCO_Id { get; set; }
        public long AMB_Id { get; set; }
        public long AMSE_Id { get; set; }
        public long ACMS_Id { get; set; }
        public string ACSDE_DeactivatedReason { get; set; }
        public DateTime? ACSDE_DeactivatedDate { get; set; }
        public bool ACSDE_ActivedFlg { get; set; }
        public DateTime? ACSDE_ActivatedDate { get; set; }
        public string ACSDE_ActivatedReason { get; set; }
        public bool ACSDE_ActiveFlag { get; set; }
        public long ACSDE_CreatedBy { get; set; }
        public long ACSDE_UpdatedBy { get; set; }
        public long userid { get; set; }
        public Array yearlist { get; set; }
        public Array courselist { get; set; }
        public Array branchlist { get; set; }
        public Array semesterlist { get; set; }
        public Array sectionlist { get; set; }
        public Array studentlist { get; set; }
        public string AMCST_SOL { get; set; }
        public string AMCST_SOL_activate { get; set; }
        public bool returnval { get; set; }
        public bool checkedvalue { get; set; }
        public string message { get; set; }
        public string remarks { get; set; }
        public Array getreport { get; set; }
        public CollegeActiveDeactiveStudentsDTO[] savetmpdata { get; set; }
    }
}
